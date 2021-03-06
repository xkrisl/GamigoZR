﻿using System;
using System.IO;
using System.Net;
using System.ServiceProcess;

namespace Gamigo_Zone_Response_Service
{
    public partial class Main : ServiceBase
    {
        public static string responseurl = ("http://127.0.0.1:58492/");
        public static string responsestring = (
            "[10000][10][{( USA V.0.001 )}]\n" +
            "73B2CCC77269D6B77284E8AC733D962D5839326E5677672C9BCEB73CD8A7276A8\n" +
            "CA5072CC7271513D8473A7E77075D5B7567331216771DC385B76EF052074E1D6C\n" +
            "F746C9D6B74FE459974BE44CD76A4A6017449A67D762AFC43748E0CE07440C61D\n" +
            "62429D2735DB19C72ABF51C717B2C5D736FEBEA75F82B18674E9277311FAB1764\n" +
            "B7FF475DE5E7576C5AD1D71678DA9778B78A5656824F7768A47E72861EF375285\n" +
            "F0676854B42776CCCF875E27241759C1E73760609BA760822BA722CB84E75C000\n" +
            "A675173FD476504850760BB2ED6DB2F9A711BA658778B80DB76A13C8A77749470\n" +
            "760822BA7281A6587690572177213C1773959F5572444A4675F2ACFA76E7FDB26\n" +
            "5CF34C73F335C7746E790375A8735E71D6D0387642B3F072E61883740BF411747\n" +
            "059C876705809757D470E726BAF4B76C1B406750D9F297281375173F11E8A65E6\n" +
            "B9972AA6A1C74663DED75C9D7D976568C4F7268A393740A5277687BD89732E11E\n" +
            "17376135F7642D55751E42971E14AA2773B00C5768F936973EF18887336E51073\n" +
            "D476B47175048974C722BE74A923C67230998569FC0067404078E71F5462C7678\n" +
            "AD3476A96A7575C0674072D373DA740A2FD472ABC3DA723D8CB97725144372BF4\n" +
            "F27736B361D72F303C776A174D87437BD227138B117734D7FC7713EED1574B934\n" +
            "18722522857330F0CC734062AA777E375D753D3A07764C71D77179CB7674ECA2A\n" +
            "3731543C1741DB21C76C39EF97450F3E676F486BA76B899617728F0BA75F78D0C\n" +
            "714174E8723A0D6B74F340E675E3163E71BE2DAC744A8EA773431BF274FC5BB07\n" +
            "4FC35FC774AB9C36FA1953652999D719D5DB376E578CD726B8A8567801017679D\n" +
            "A427528AD0B6C8782461C67FA72DB068974601C1A74CFA65871BB2678648CB097\n" +
            "5541AAB71F16EF2774BC58C75EA7057739F003F72A504247654F3A575B3632B76\n" +
            "9F5F1A74CE439675A8B900751E51FB74F9028A738D9B9F7376F26C723EB92171C\n" +
            "395BC76BFC75B7779245175367E7971625031713E984171BC1493723F6705741C\n" +
            "68C576DCEFBC67F5E3164882F571E73C2771A9EFD961070F47642F7C672578D4E\n" +
            "751E51FB7292173D7107186174CB8E8272E6718973F06A8D72380F6575EB397E7\n" +
            "3B74FB5746E5B2672D53A5D6B9A12474B3766A74E8C0FD6DCB610756EE5CC61DD\n" +
            "A6574F85F487335EB67767B0C31623654E73F00A9F56FF9872BC40AF6D058B074\n" +
            "948357777C83FB72764B6D767AFC5C7280A69F7672D2BB72D8B3DC72CB822A715\n" +
            "E7679720CA2AA75BD29E0723A2A34756F2005734DF2B6769C8EFD72BDCC446A4F\n" +
            "5C074D3827C7197679F72375EA7723C7E1572C411DC73ED2F7E75A7BD9262AA16\n" +
            "D759BDE8672F49898722522857506BFDE77374554730E09F4757163C1726FF272\n" +
            "76B4E794720D83A07753DCF87422C98567A774876B3CF7C7789077C735111346F\n" +
            "2A31871FC498D728B4AE97477791471B07EE3761A8EAF756321F275E3919E742B\n" +
            "88295F7B7A751FC59D6DD5BB175CF379D778D6B627667C6CA73057C9C72752534\n" +
            "757C9EEE7167A883771FF10872E7832071932E2C7533E282775611947379E1C77\n" +
            "6245B937158232671FBDB62723167BE7127319F7375867C73FE40A3771F856275\n" +
            "E02C867144C8B172853F23740E9DAC7783D7F072F8B10B6763DEF719828D6752D\n" +
            "40AF717E157974F9E31A71D5181F7620653173A757457670A6367350EDF576F14\n" +
            "E93612F7117408B90476C133377381DB4F7673D347745903F0768531F776BC688\n" +
            "771AD13F4723C7E1573556FD8775AEAF77140DE077449A1B97738315373B6130A\n" +
            "738217AF75E8953D7116D7A4761FFA1F71C4F63775AA79A3765AC30B7280A9587\n" +
            "4903026713C2FDA\n"
            );

        public bool isRunning = false;
        public Responder ws;
        public Main()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (!this.isRunning)
            {
                this.ws = new Responder(new Func<HttpListenerRequest, string>(SendResponse), new string[1]
                {
                  responseurl
                });
                try
                {
                    this.ws.Run(responseurl);
                    this.isRunning = true;
                }
                catch (Exception ex)
                {
                    string path = @"./ExceptionError.txt";
                    using (StreamWriter sw = new StreamWriter(path, true))
                    {
                        sw.Write(string.Format("Message: {0}<br />{1}StackTrace :{2}{1}Date :{3}{1}-----------------------------------------------------------------------------{1}", ex.Message, Environment.NewLine, ex.StackTrace, DateTime.Now.ToString()));
                    }
                }
            }
            else
            {
                Stop();
            }
        }

        public static string SendResponse(HttpListenerRequest request)
        {
            return responsestring;
        }
    }
}