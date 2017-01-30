using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apis.Client
{
    public class PostData
    {

        private List<PostDataParam> m_Params;

        public List<PostDataParam> Params
        {
            get { return m_Params; }
            set { m_Params = value; }
        }

        public PostData()
        {
            m_Params = new List<PostDataParam>();
        }


        /// <summary>
        /// Returns the parameters array formatted for multi-part/form data
        /// </summary>
        /// <returns></returns>
        public byte[] GetPostData(string boundary)
        {
            List<byte> sb = new List<byte>();

            foreach (PostDataParam p in m_Params)
            {
                sb.AddRange(UTF8Encoding.UTF8.GetBytes(boundary));

                if (p.Type == PostDataParamType.File)
                {
                    sb.AddRange(UTF8Encoding.UTF8.GetBytes(Environment.NewLine));
                    sb.AddRange(UTF8Encoding.UTF8.GetBytes(string.Format("Content-Disposition: file; name=\"{0}\"; filename=\"{1}\"", p.Name, String.Format("{0}", p.FileName))));

                    sb.AddRange(UTF8Encoding.UTF8.GetBytes(Environment.NewLine));
                    sb.AddRange(UTF8Encoding.UTF8.GetBytes("Content-Type: image/jpeg"));

                    sb.AddRange(UTF8Encoding.UTF8.GetBytes(Environment.NewLine));
                    sb.AddRange(UTF8Encoding.UTF8.GetBytes(Environment.NewLine));
                    sb.AddRange(p.BValue);
                }
                else
                {
                    sb.AddRange(UTF8Encoding.UTF8.GetBytes(Environment.NewLine));
                    sb.AddRange(UTF8Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"", p.Name)));

                    sb.AddRange(UTF8Encoding.UTF8.GetBytes(Environment.NewLine));
                    sb.AddRange(UTF8Encoding.UTF8.GetBytes(Environment.NewLine));
                    sb.AddRange(UTF8Encoding.UTF8.GetBytes(p.Value));
                }

                sb.AddRange(UTF8Encoding.UTF8.GetBytes(Environment.NewLine));
            }

            sb.AddRange(UTF8Encoding.UTF8.GetBytes(Environment.NewLine));
            sb.AddRange(UTF8Encoding.UTF8.GetBytes(boundary));

            return sb.ToArray();
        }
    }

    public enum PostDataParamType
    {
        Field,
        File
    }

    public class PostDataParam
    {


        public PostDataParam(string name, string value, PostDataParamType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        public PostDataParam(string name, byte[] value, PostDataParamType type)
        {
            Name = name;
            BValue = value;
            Type = type;
        }

        public string Name;
        public string FileName;
        public string Value;
        public PostDataParamType Type;
        public byte[] BValue;
    }
}
