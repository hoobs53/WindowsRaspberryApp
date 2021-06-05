using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace BigApp.ViewModel
{
    using Model;
    public class ConfigViewModel : BaseViewModel
    {
        ConfigParams config;

        private string url;
        public string Url
        {
            get { return url; }
            set
            {
                if (url != value)
                {
                    url = value;
                    OnPropertyChanged("Url");
                }
            }
        }

        private int sampleTime;
        public string SampleTime
        {
            get { return sampleTime.ToString(); }
            set
            {
                if (Int32.TryParse(value, out int st))
                {
                    if (sampleTime != st)
                    {
                        sampleTime = st;
                        OnPropertyChanged("SampleTime");
                    }
                }
            }
        }

        private string api;
        public string Api
        {
            get { return api; }
            set
            {
                if (api != value)
                {
                    api = value;
                    OnPropertyChanged("Api");
                }
            }
        }

        private string port;
        public string Port
        {
            get { return port; }
            set
            {
                if (port != value)
                {
                    port = value;
                    OnPropertyChanged("Port");
                }
            }
        }

        public ButtonCommand SetParams { get; set; }

        public ConfigViewModel()
        {
            config = new ConfigParams();
            Api = config.Api;
            Port = config.Port;
            SampleTime = config.SampleTime.ToString();
            Url = config.Url;
            SetParams = new ButtonCommand(SaveParams);
        }
        // Saving parameters to JSON file
        private void SaveParams()
        {
            var data = new Dictionary<string, string>();
            data.Add("api",Api);
            data.Add("sample_time", SampleTime);
            data.Add("port", Port);
            data.Add("url", Url);
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText("config.json", json);
        }
    }
}
