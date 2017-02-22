using System;
using System.Collections.Generic;
using System.Linq;

namespace GL.HttpServer.Types
{
    public class ParameterBindInfo
    {
        public ParameterBindInfo(string mask, string parameterName, Type parameterType)
        {
            Mask = mask;
            ParameterType = parameterType;
            var delimiter = "{" + parameterName + "}";
            var segments = mask.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            if(segments.Count == 2)
            {
                MaskSegments = segments;
            }
        }

        public List<string> MaskSegments { get; set; }

        public string Mask { get; set; }

        public Type ParameterType { get; set; }
    }
}
