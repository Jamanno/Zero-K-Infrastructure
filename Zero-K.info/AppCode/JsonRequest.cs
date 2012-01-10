﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace ZeroKWeb
{
    public class JsonRequest
    {
        public static string MakeRequest(string url, object data) {

            var ser = new JavaScriptSerializer();
            var serialized = ser.Serialize(data);
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            //request.ContentType = "application/json; charset=utf-8";
            request.ContentType = "text/html; charset=utf-8";
            request.ContentLength = serialized.Length;

            StreamWriter writer = new StreamWriter(request.GetRequestStream());

            writer.Write(serialized);
            writer.Close();
            var ms = new MemoryStream();
            request.GetResponse().GetResponseStream().CopyTo(ms);
            return serialized;
        }

    }
}