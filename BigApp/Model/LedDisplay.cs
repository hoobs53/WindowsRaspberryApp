using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace BigApp.Model
{
    public class LedDisplay
    {
        public readonly int SizeX = 8;
        public readonly int SizeY = 8;
        public string status = "";

        public byte ActiveColorA    //!< Active color Alpha components
        {
            get => (byte)255;
        }

        private byte _activeColorR;
        public byte ActiveColorR    //!< Active color Red components
        {
            set => _activeColorR = value;
        }

        private byte _activeColorG;
        public byte ActiveColorG    //!< Active color Green components
        {
            set => _activeColorG = value;
        }

        private byte _activeColorB;
        public byte ActiveColorB    //!< Active color Blue components
        {
            set => _activeColorB = value;
        }

        public Color ActiveColor    //!< Active color in ARG format
        {
            get => Color.FromArgb(ActiveColorA, _activeColorR, _activeColorG, _activeColorB);
        }


        public readonly Color OffColor;   //!< 'LED-is-off' color in Int ARGB format

        private UInt16?[,,] model;
        private UInt16?[,,] currentModel;

        public LedDisplay(int offColor)
        {
            model = new UInt16?[SizeX, SizeY, 3];
            currentModel = new UInt16?[SizeX, SizeY, 3];
            OffColor = Color.FromArgb(255, 0, 0, 0);
            _activeColorR = OffColor.R;
            _activeColorG = OffColor.G;
            _activeColorB = OffColor.B;
            ClearModel();
        }

        private JArray IndexToJsonArray(int x, int y)
        {
            JArray array = new JArray();
            try
            {
                array.Add(x);
                array.Add(y);
                array.Add(model[x, y, 0]);
                array.Add(model[x, y, 1]);
                array.Add(model[x, y, 2]);
            }
            catch (JsonException e)
            {
                Trace.TraceError(e.Message);
            }
            return array;
        }

        public void UpdateModel(int x, int y, byte r, byte g, byte b)
        {
            model[x, y, 0] = r;
            model[x, y, 1] = g;
            model[x, y, 2] = b;

            CheckIfChanged();
        }
        public void initDisplay()
        {
            ushort? r, g, b;
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {

                    r = model[i, j, 0];
                    g = model[i, j, 1];
                    b = model[i, j, 2];
                    currentModel[i, j, 0] = r;
                    currentModel[i, j, 1] = g;
                    currentModel[i, j, 2] = b;
                }
            }
        }
        public void CheckIfChanged()
        {
            status = "";
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    if (model[i, j, 0] != currentModel[i, j, 0] || model[i, j, 1] != currentModel[i, j, 1] || model[i, j, 2] != currentModel[i, j, 2])
                    {
                        status = "UNSAVED CHANGES*";
                    }
                }
            }    
        }

        public void ClearModel()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    model[i, j, 0] = 0;
                    model[i, j, 1] = 0;
                    model[i, j, 2] = 0;

                    currentModel[i, j, 0] = 0;
                    currentModel[i, j, 1] = 0;
                    currentModel[i, j, 2] = 0;

                }
            }
        }

        public List<KeyValuePair<string, string>> getControlPostData()
        {
            ushort? r, g, b;
            var postData = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                        postData.Add(
                            new KeyValuePair<string, string>(
                                "LED" + i.ToString() + j.ToString(),
                                IndexToJsonArray(i, j).ToString()
                                ));
                    r = model[i, j, 0];
                    g = model[i, j, 1];
                    b = model[i, j, 2];
                    currentModel[i, j, 0] = r;
                    currentModel[i, j, 1] = g;
                    currentModel[i, j, 2] = b;
                }
            }
            return postData;
        }
    }
}
