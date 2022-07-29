using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Text;

namespace vbSparkle.Web
{
        public class SimpleTextInputFormatter : TextInputFormatter
        {
            public SimpleTextInputFormatter()
            {
                SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/cu-for"));

                SupportedEncodings.Add(Encoding.UTF8);
                SupportedEncodings.Add(Encoding.Unicode);
            }


            protected override bool CanReadType(Type type)
            {
                if (type == typeof(string))
                {
                    return base.CanReadType(type);
                }
                return false;
            }

            public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                if (encoding == null)
                {
                    throw new ArgumentNullException(nameof(encoding));
                }

                var request = context.HttpContext.Request;

                using (var reader = new StreamReader(request.Body, encoding))
                {
                    try
                    {
                        var text = await reader.ReadToEndAsync();
                        

                        return await InputFormatterResult.SuccessAsync(text);
                    }
                    catch
                    {
                        return await InputFormatterResult.FailureAsync();
                    }
                }
            }
        }
    }

