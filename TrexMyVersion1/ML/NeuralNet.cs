using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using TrexMyVersion1.Objects;

namespace TrexMyVersion1.ML
{
    public class NeuralNet
    {
        #region data
        //input layer, hidden layer, output layer
        public int input_amount { get; set; }
        public int hidden_amount { get; set; }
        public int output_amount { get; set; }

        //connect input-hidden
        public float[,] kin { get; set; }
        public float[] bin { get; set; }

        //connect hid-out
        public float[,] kout { get; set; }
        public float[] bout { get; set; }
        #endregion
        #region ctor

        public NeuralNet(int inputA, int hiddenA, int outputA, float[,] kin, float[] bin, float[,] kout, float[] bout)
        {
            //amounts
            this.input_amount = inputA;
            this.hidden_amount = hiddenA;
            this.output_amount = outputA;

            //in
            for (int x = 0; x < kin.GetLength(0); x++)
            {
                for (int y = 0; y < kin.GetLength(1); y++)
                {
                    this.kin[x, y] = kin[x, y];
                }
            }
            for (int i = 0; i < bin.Length; i++)
            {
                this.bin[i] = bin[i];
            }

            //out
            for (int x = 0; x < kout.GetLength(0); x++)
            {
                for (int y = 0; y < kout.GetLength(1); y++)
                {
                    this.kout[x, y] = kout[x, y];
                }
            }
            for (int i = 0; i < bout.Length; i++)
            {
                this.bin[i] = bout[i];
            }
        }
        
        /// <summary>
        /// basic constructor for a neural net
        /// </summary>
        /// <param name="Inputnum"> int input amount</param>
        /// <param name="Hiddennum">int, hidden size</param>
        /// <param name="Outputnum">int output amount</param>
        public NeuralNet(int Inputnum, int Hiddennum, int Outputnum)
        {
            //amounts
            this.input_amount = Inputnum;
            this.hidden_amount = Hiddennum;
            this.output_amount = Outputnum;

            //setting arrays
            kin = new float[Inputnum, Hiddennum];
            bin = new float[Hiddennum];
            kout = new float[Hiddennum, Outputnum];
            bout = new float[Outputnum];

            //filling arraays with random numbers
            for (int i = 0; i < Inputnum; i++)
                for (int j = 0; j < Hiddennum; j++)
                    kin[i, j] = getrandomfloat();
            for (int i = 0; i < Hiddennum; i++)
                bin[i] = getrandomfloat();
            for (int i = 0; i < Hiddennum; i++)
                for (int j = 0; j < Outputnum; j++)
                    kout[i, j] = getrandomfloat();
            for (int i = 0; i < Outputnum; i++)
                bout[i] = getrandomfloat();
        }
        public NeuralNet() { }
        //copy constractor
        public NeuralNet(NeuralNet nn)
        {
            this.input_amount = nn.input_amount;
            this.hidden_amount = nn.hidden_amount;
            this.output_amount = nn.output_amount;
            kin = new float[input_amount, hidden_amount];
            bin = new float[hidden_amount];
            kout = new float[hidden_amount, output_amount];
            bout = new float[output_amount];
            for (int i = 0; i < input_amount; i++)
                for (int j = 0; j < hidden_amount; j++)
                    kin[i, j] = nn.kin[i, j];
            for (int i = 0; i < hidden_amount; i++)
                bin[i] = nn.bin[i];
            for (int i = 0; i < hidden_amount; i++)
                for (int j = 0; j < output_amount; j++)
                    kout[i, j] = nn.kout[i, j];
            for (int i = 0; i < output_amount; i++)
                bout[i] = nn.bout[i];

        }

        //from data
        /// <summary>
        /// parsing neual net data from txt file in givven location
        /// </summary>
        /// <param name="loc">string location + file</param>
        public NeuralNet(String loc)
        {
            string content = File.ReadAllText(loc);

            #region amounts
            //amount of inputs
            int stop = 1;
            for (int i = 1; i < content.Length; i++)
            {
                stop = i;
                if (content[i] == char.Parse(";"))
                {
                    break;
                }
            }
            string parse = content.Substring(1, stop-1);
            this.input_amount = int.Parse(parse);

            //amount of hiddens
            int stop2 = stop;
            for (int i = stop+1; i < content.Length; i++)
            {
                stop2 = i;
                if (content[i] == char.Parse(";"))
                {
                    break;
                }
            }
            parse = content.Substring(stop+1, stop2 - stop-1);
            this.hidden_amount = int.Parse(parse);

            //amount of outputs
            stop = stop2;
            for (int i = stop + 1; i < content.Length; i++)
            {
                stop2 = i;
                if (content[i] == char.Parse(";"))
                {
                    break;
                }
            }
            parse = content.Substring(stop + 1, stop2-stop-1);
            this.output_amount = int.Parse(parse);
            #endregion

            //setting arrays
            kin = new float[input_amount, hidden_amount];
            bin = new float[hidden_amount];
            kout = new float[hidden_amount, output_amount];
            bout = new float[output_amount];

            // k in
            
            for (int h = 0; h < this.hidden_amount; h++)
            {
                stop = stop2 + 1;
                for (int w = 0; w < this.input_amount; w++)
                {
                    for (int i = stop + 1; i < content.Length; i++)
                    {
                        stop2 = i;
                        if (content[i] == char.Parse(",") || content[i] == char.Parse(")"))
                        {
                            break;
                        }
                    }
                    parse = content.Substring(stop + 1, stop2 - stop - 1);
                    kin[w, h] = float.Parse(parse);
                    stop = stop2;
                    if (content[stop2] == char.Parse(")"))
                        break;
                }
                if (content[stop2+1] == char.Parse(";"))
                    break;
            }

            //b in
            stop = stop2 + 2;
            for (int i = 0; i < this.hidden_amount; i++)
            {
                for (int t = stop+1; t < content.Length; t++)
                {
                    stop2 = t;
                    if (content[t] == char.Parse(",") || content[t] == char.Parse(")"))
                    {
                        break;
                    }
                }

                parse = content.Substring(stop + 1, stop2 - stop - 1);
                bin[i] = float.Parse(parse);
                stop = stop2;
                if (content[stop2] == char.Parse(")"))
                    break;
            }
            stop2++;

            //k out
            for (int h = 0; h < this.output_amount; h++)
            {
                stop = stop2 + 1;
                for (int w = 0; w < this.hidden_amount; w++)
                {
                    for (int i = stop + 1; i < content.Length; i++)
                    {
                        stop2 = i;
                        if (content[i] == char.Parse(",") || content[i] == char.Parse(")"))
                        {
                            break;
                        }
                    }
                    parse = content.Substring(stop + 1, stop2 - stop - 1);
                    kout[w, h] = float.Parse(parse);
                    stop = stop2;
                    if (content[stop2] == char.Parse(")"))
                        break;
                }
                if (content[stop2 + 1] == char.Parse(";"))
                    break;
            }

            //b out
            stop = stop2 + 2;
            for (int i = 0; i < this.output_amount; i++)
            {
                for (int t = stop + 1; t < content.Length; t++)
                {
                    stop2 = t;
                    if (content[t] == char.Parse(",") || content[t] == char.Parse(")"))
                    {
                        break;
                    }
                }

                parse = content.Substring(stop + 1, stop2 - stop - 1);
                bout[i] = float.Parse(parse);
                stop = stop2;
                if (content[stop2] == char.Parse(")"))
                    break;
            }

        }
        #endregion

        #region funcs

        /// <summary>
        /// getting random float between -5 and 5
        /// </summary>
        /// <returns></returns>
        private float getrandomfloat()
        {
            return 0.1f * (Tools.rnd.Next(100) - 50);
        }

        #region calculate

        /// <summary>
        /// using the Tanh on a number in order to get a "y"/ a new number between -1 and 1
        /// and breaking the linear connection in the neural network so far
        /// </summary>
        /// <param name="x">number</param>
        /// <returns></returns>
        private float breakLin(float x)
        {
            return (float)(Math.Tanh(x / 10));
        }

        /// <summary>
        /// calculating the network
        /// </summary>
        /// <param name="input">float[], inputs for the network to work with</param>
        /// <returns></returns>
        public float[] calculate(float[] input)
        {
            float[] hid = new float[hidden_amount];
            for (int i = 0; i < hidden_amount; i++)
            {
                hid[i] = bin[i];
                for (int j = 0; j < input_amount; j++)
                {
                    hid[i] += input[j] * kin[j, i];
                }
                hid[i] = breakLin(hid[i]);
            }
            float[] outt = new float[output_amount];
            for (int i = 0; i < output_amount; i++)
            {
                outt[i] = bout[i];
                for (int j = 0; j < hidden_amount; j++)
                {
                    outt[i] += hid[j] * kout[j, i];
                }
                outt[i] = breakLin(outt[i]);
            }
            return outt;
        }
        #endregion

        #region mutate
        /// <summary>
        /// creating a new web
        /// </summary>
        public void changeAlot()
        {
            for (int i = 0; i < input_amount; i++)
                for (int j = 0; j < hidden_amount; j++)
                    kin[i, j] = getrandomfloat();
            for (int i = 0; i < hidden_amount; i++)
                bin[i] = getrandomfloat();
            for (int i = 0; i < hidden_amount; i++)
                for (int j = 0; j < output_amount; j++)
                    kout[i, j] = getrandomfloat();
            for (int i = 0; i < output_amount; i++)
                bout[i] = getrandomfloat();
        }

        /// <summary>
        /// mutating with slite random difrences
        /// </summary>
        public void mutate()
        {
            for (int i = 0; i < input_amount; i++)
                for (int j = 0; j < hidden_amount; j++)
                    kin[i, j] = change_a_bit(kin[i, j]);
            for (int i = 0; i < hidden_amount; i++)
                bin[i] = change_a_bit(bin[i]);
            for (int i = 0; i < hidden_amount; i++)
                for (int j = 0; j < output_amount; j++)
                    kout[i, j] = change_a_bit(kout[i, j]);
            for (int i = 0; i < output_amount; i++)
                bout[i] = change_a_bit(bout[i]);
        }

        /// <summary>
        /// changing a bit a given float
        /// </summary>
        /// <param name="x">float</param>
        /// <returns></returns>
        float change_a_bit(float x)
        {
            int r = Tools.rnd.Next(3);
            if (r == 0) return x;
            if (r == 1) return mutate2(x);
            return mutate1(x);
        }

        /// <summary>
        /// first kinf of change 
        /// </summary>
        /// <param name="x">float</param>
        /// <returns></returns>
        float mutate1(float x)
        {
            float f = (Tools.rnd.Next(20) - 5) * 0.05f; //-0.25 - 0.25
            return f + x;
        }

        /// <summary>
        /// second kind of change
        /// </summary>
        /// <param name="x">float</param>
        /// <returns></returns>
        float mutate2(float x)
        {
            float f = (Tools.rnd.Next(20) - 5) * 0.05f;
            return (f + 1) * x;
        }

        #endregion

        #region read write
        public override String ToString()
        {
            String ret = ";" + input_amount + ";" + hidden_amount + ";" + output_amount+";";
            ret += Tools.ToString_two_dimention_array(this.kin)+";" +Tools.ToString_array(this.bin) +";"+
                Tools.ToString_two_dimention_array(this.kout)+";"
                + Tools.ToString_array(this.bout);
            return ret;
        }

        public NeuralNet Load(string file)
        {
            NeuralNet ret = new NeuralNet();
            XmlSerializer formatter = new XmlSerializer(ret.GetType());
            FileStream nFile = new FileStream(file, FileMode.Open);
            byte[] buffer = new byte[nFile.Length];
            nFile.Read(buffer, 0, (int)nFile.Length);
            MemoryStream stream = new MemoryStream(buffer);
            return (NeuralNet)formatter.Deserialize(stream);
        }


        public void Save(string path, NeuralNet nn)
        {
            FileStream outFile = File.OpenWrite(path);
            XmlSerializer formatter = new XmlSerializer(nn.GetType());
            formatter.Serialize(outFile, nn);
        }

        #endregion

        #endregion
    }
}
