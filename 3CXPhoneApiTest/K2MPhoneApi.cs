using System;
using MyPhonePlugins;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

namespace _UniveraCallHandler
{
    [CRMPluginLoader]
    public class K2MPhoneApi
    {
        private static K2MPhoneApi _phoneApiInstance = null;
        private readonly IMyPhoneCallHandler _callHandler;
        private bool controlled = false;
        private string controlRes = null;
        private string callId = null;

        [CRMPluginInitializer]
        public static void Loader(IMyPhoneCallHandler callHandler)
        {
            _phoneApiInstance = new K2MPhoneApi(callHandler);
        }

        private K2MPhoneApi(IMyPhoneCallHandler callhandler)
        {
            _callHandler = callhandler;
            callhandler.OnCallStatusChanged += Callhandler_OnCallStatusChanged;
        }

        private async void Callhandler_OnCallStatusChanged(object sender, CallStatus callInfo)
        {
            if (callInfo.OriginatorType == OriginatorType.Queue && callInfo.Incoming &&
                callInfo.State == CallState.Connected && !callInfo.IsHold && !callInfo.IsMuted &&
                callInfo.CallID != callId)
            {
                // using (StreamWriter sw = new StreamWriter(@"c:\temp\connected.txt",true))
                // {
                //     sw.WriteLine("res : {0}  controlled: {1}",controlRes,controlled);
                // }

                callId = callInfo.CallID;
                controlled = false;

                Process.Start("http://10.41.1.33:8182/Home/CallerRedirectPage?CallerId=" + callInfo.OtherPartyNumber);
            }
            else if (callInfo.OriginatorType == OriginatorType.Queue && callInfo.Incoming &&
                     callInfo.State == CallState.Ringing && !controlled)
            {
                controlled = true;
            }
        }

        // if (callInfo.OriginatorType == OriginatorType.Queue && callInfo.Incoming &&
        //     callInfo.State == CallState.Connected && !callInfo.IsHold && !callInfo.IsMuted && callInfo.CallID!=callId)
        // {
        //     
        //     // using (StreamWriter sw = new StreamWriter(@"c:\temp\connected.txt",true))
        //     // {
        //     //     sw.WriteLine("res : {0}  controlled: {1}",controlRes,controlled);
        //     // }
        //     
        //     callId = callInfo.CallID;
        //     controlled = false;
        //     
        //     if (controlRes=="ok")
        //     {
        //         Process.Start("http://10.41.1.33:8182/Home/CallerRedirectPage?CallerId=" + callInfo.OtherPartyNumber);
        //     }
        //     
        // }
        //
        // else if (callInfo.OriginatorType == OriginatorType.Queue && callInfo.Incoming &&
        //          callInfo.State == CallState.Ringing && !controlled)
        // {
        //    
        //     // using (StreamWriter sw = new StreamWriter(@"c:\temp\ringing.txt",true))
        //     // {
        //     //     sw.WriteLine("res : {0}  controlled: {1}",controlRes,controlled);
        //     // }
        //     using (var client = new HttpClient())
        //     {
        //         controlled = true;
        //         var response = await client.GetAsync(
        //             "http://10.41.3.95:8089/service/data/makecall/checkpostdata/"+callInfo.OtherPartyNumber);
        //         //  AudioURL deserializedObject= JsonConvert.DeserializeObject<AudioURL>(response.Content.ReadAsStringAsync().Result);
        //         var res = response.Content.ReadAsStringAsync().Result;
        //         controlRes=res;
        //         
        //         // using (StreamWriter sw = new StreamWriter("c:\\json.txt", true))
        //         // {
        //         //     sw.WriteLine(response.Content.ReadAsStringAsync().Result + " " + DateTime.Now);
        //         //     //  sw.WriteLine(DateTime.Now);
        //         // }
        //         
        //     }
        // }
    }
}