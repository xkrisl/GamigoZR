using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Gamigo_Zone_Response_Service
{
    public class Responder
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public Responder(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
            if ((prefixes == null ? 1 : (prefixes.Length == 0 ? 1 : 0)) != 0)
                throw new ArgumentException(nameof(prefixes));
            foreach (string prefix in prefixes)
                this._listener.Prefixes.Add(prefix);
            Func<HttpListenerRequest, string> func = method;
            this._responderMethod = func ?? throw new ArgumentException(nameof(method));
            this._listener.Start();
        }

        public Responder(Func<HttpListenerRequest, string> method, params string[] prefixes)
          : this(prefixes, method)
        {
        }

        public void Run(string url)
        {
            ThreadPool.QueueUserWorkItem((WaitCallback)(o =>
            {
                try
                {
                    while (this._listener.IsListening)
                        ThreadPool.QueueUserWorkItem((WaitCallback)(c =>
                        {
                            HttpListenerContext httpListenerContext = c as HttpListenerContext;
                            try
                            {
                                byte[] bytes = Encoding.UTF8.GetBytes(this._responderMethod(httpListenerContext.Request));
                                httpListenerContext.Response.ContentLength64 = (long)bytes.Length;
                                httpListenerContext.Response.OutputStream.Write(bytes, 0, bytes.Length);
                                string end;
                                using (Stream inputStream = httpListenerContext.Request.InputStream)
                                {
                                    using (StreamReader streamReader = new StreamReader(inputStream, Encoding.UTF8))
                                        end = streamReader.ReadToEnd();
                                }
                            }
                            catch (Exception ex)
                            {
                                string path = @"./ExceptionError.txt";
                                using (StreamWriter sw = new StreamWriter(path, true))
                                {
                                    sw.Write(string.Format("Message: {0}<br />{1}StackTrace :{2}{1}Date :{3}{1}-----------------------------------------------------------------------------{1}", ex.Message, Environment.NewLine, ex.StackTrace, DateTime.Now.ToString()));
                                }
                            }
                            finally
                            {
                                httpListenerContext.Response.OutputStream.Close();
                            }
                        }), (object)this._listener.GetContext());
                }
                catch (Exception ex)
                {
                    string path = @"./ExceptionError.txt";
                    using (StreamWriter sw = new StreamWriter(path, true))
                    {
                        sw.Write(string.Format("Message: {0}<br />{1}StackTrace :{2}{1}Date :{3}{1}-----------------------------------------------------------------------------{1}", ex.Message, Environment.NewLine, ex.StackTrace, DateTime.Now.ToString()));
                    }
                }
            }));
        }
    }
}