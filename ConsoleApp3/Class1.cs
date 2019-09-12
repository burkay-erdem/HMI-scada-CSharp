using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBoxClientDriver;
using FBoxClientDriver.Contract;
using FBoxClientDriver.Impl;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.ComponentModel;
using System.Data.SqlClient;
namespace ConsoleApp3
{
    public class FBoxClientParameters
    {
        public static string ClientId { get; set; } = "ebae";
        public static string ClientSecret { get; set; } = "2def71770779de31ba196d9423735368";
        public static string UserName { get; set; } = "albaelektronik";
        public static string Password { get; set; } = "root1234";
        public static string IdServer { get; set; } = "https://account.flexem.com/core";
        public static string MainServer { get; set; } = "http://fbox360.com";
        public static string HdataServer { get; set; } = "http://fbhs1.fbox360.com";

        public static string BOXNO { get; set; }
        public static string makinahız { get; set; } 
        public static string makinadevir { get; set; } 
        public static string setdeg { get; set; } 
        public static string anlıkdeg { get; set; }
        public static string bekdk { get; set; }
        public static string beksn { get; set; }
        public static string çaldk { get; set; }
        public static string çalsn { get; set; }

        public static string altlamb { get; set; }
        public static string üstlamb { get; set; }
        public static string fan { get; set; }
        public static string ipmud { get; set; }
       

    }
    public class FBoxDemo : IDisposable
    {
        private readonly IFBoxClientManager _fbox;
        private readonly ILogger<FBoxDemo> _logger;
        public FBoxDemo(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FBoxDemo>();
            ICredentialProvider provider =
            new DefaultCredentialProvider(FBoxClientParameters.ClientId, FBoxClientParameters.ClientSecret, FBoxClientParameters.UserName, FBoxClientParameters.Password);
            _fbox = new FBoxClientManager(FBoxClientParameters.IdServer, FBoxClientParameters.MainServer, FBoxClientParameters.HdataServer, provider, Guid.NewGuid().ToString("N"), loggerFactory);
            _fbox.BoxConnectStateChanged += _fbox_BoxConnectStateChanged;
            _fbox.DataMonitorValueChanged += _fbox_DataMonitorValueChanged;
        }
        int i = 0;
        private void _fbox_DataMonitorValueChanged(object sender, IList<DataMonitorValueChangedArgs> e)
        {
           
            
                Console.WriteLine(String.Format("{0,10} \t | {1,20} \t | {2,15} \t | {3,15} \t ",
                         "boxid", "value", "time", "boxno"));
                foreach (var dmon in e)
                {
                   
                    Console.WriteLine(String.Format("{0,10} \t | {1,20} \t | {2,10} \t | {3,10} \t ",
                        dmon.Name, dmon.Value, dmon.Status,dmon.Timestamp));
                FBoxClientParameters.BOXNO = dmon.BoxNo;
                if (dmon.Name == "makinadevir")
                    FBoxClientParameters.makinadevir = dmon.Value.ToString();
                if (dmon.Name == "makinahızı" && dmon.Value!=null)
                    FBoxClientParameters.makinahız = dmon.Value.ToString();
                if (dmon.Name == "set_deg")
                    FBoxClientParameters.setdeg = dmon.Value.ToString();
                if (dmon.Name == "anlık_deg")
                    FBoxClientParameters.anlıkdeg = dmon.Value.ToString();
                if (dmon.Name == "beklme_dk")
                    FBoxClientParameters.bekdk = dmon.Value.ToString();
                if (dmon.Name == "bekleme_sn")
                    FBoxClientParameters.beksn = dmon.Value.ToString();
                if (dmon.Name == "calıs_dk")
                    FBoxClientParameters.çaldk = dmon.Value.ToString();
                if (dmon.Name == "calıs_sn")
                    FBoxClientParameters.çalsn = dmon.Value.ToString();
                if (dmon.Name == "alt_lamba")
                    FBoxClientParameters.altlamb = dmon.Value.ToString();
                if (dmon.Name == "ust_lamba")
                    FBoxClientParameters.üstlamb = dmon.Value.ToString();
                if (dmon.Name == "ip_mud")
                    FBoxClientParameters.ipmud = dmon.Value.ToString();
                if (dmon.Name == "fan")
                    FBoxClientParameters.fan = dmon.Value.ToString();
                


            }
        }
  
        private void _fbox_BoxConnectStateChanged(object sender, IList<BoxConnectionStateItem> e)
        {
            Task.Run(async () =>
            {
                foreach (var stateItem in e)
                {
                    if (stateItem.NewState == BoxConnectionState.Connected || stateItem.NewState == BoxConnectionState.TimedOut)
                    {
                        try
                        {
                            await _fbox.StartAllDataMonitorPointsOnBox(new BoxArgs(stateItem.BoxNo));
                        }
                        catch (Exception ex)
                        {
                            switch (ex.Message)
                            {
                                case "Box server not found.":; break;
                                case "Not Found":; break;
                                default:; break;
                            }
                            Console.WriteLine(ex);
                        }
                    }
                }
            });
        }
        public async Task Go()
        {
            try
            {
                await _fbox.Restart();
            }
            catch (LoginFailedException e)
            {
            }
            catch (Exception e)
            {
            };
            await WriteDmonValueAsync();
        }
        public async Task WriteDmonValueAsync()
        {
            try
            {
                await _fbox.WriteValue(new DataMonitorWriteValueArgsV2()
                {
                    BoxNo = FBoxClientParameters.BOXNO,
                    DataMonitorUid = 130012723964960506,
                    Type = WriteValueType.Decimal,
                    Value = "35"
                });
            }
            catch (Exception e)
            {
                switch (e.Message)
                {
                    // 当前账号下，没有序列号为BoxNo的盒子
                    case "Not Found":; break;
                    // 监控点Id或者监控点名称，在BoxNo盒子下不存在
                    case "监控点条目不存在":; break;
                    // 监控点配置为只读，无法写入
                    case "监控点不可写入":; break;
                    // 序列号为BoxNo的盒子不在线
                    case "FBox会话不存在":; break;
                    // value参数超出范围
                    case "参数格式不正确":; break;
                    default:; break;
                }
                Console.WriteLine(e.Message);
            }
        }
        public void Dispose()
        {
            _fbox.BoxConnectStateChanged -= _fbox_BoxConnectStateChanged;
            _fbox.DataMonitorValueChanged -= _fbox_DataMonitorValueChanged;
            _fbox?.Dispose();
        }
    }
}
