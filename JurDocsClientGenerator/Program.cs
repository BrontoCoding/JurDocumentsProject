﻿using NSwag;
using NSwag.CodeGeneration.CSharp;
using System.Diagnostics.CodeAnalysis;

namespace JurClientGenerator
{
    internal class Program
    {
        [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
        static void Main(string[] args)
        {
            Console.WriteLine("JurDocs Client Generator!");

            var f = Run().GetAwaiter().GetResult();

            Console.WriteLine(f.Length);

            Console.Write("#");
            Console.ReadLine();
        }

        public static async Task<string> Run()
        {
            var document = await OpenApiYamlDocument.FromFileAsync(AppConst.JurDocsYamlDoc);

            var settings = new CSharpClientGeneratorSettings
            {
                ClassName = "JurDocsClient",
                CSharpGeneratorSettings = {
                    Namespace = "JurDocs.Client",
                    GenerateJsonMethods = true, 
                    //DateType="DateTime",
                    //DateTimeType="DateTime"
                },
                CodeGeneratorSettings = {
                    TemplateDirectory = "template",
                },
                GenerateResponseClasses = true,
                ResponseClass = "SwaggerResponse",
                WrapResponses = true,
                ParameterDateFormat = "yyyyMMdd",
                ParameterDateTimeFormat = "yyyyMMddHHmmss",
                GenerateDtoTypes = true,
            };

            var generator = new CSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();

            File.WriteAllText(AppConst.JurDocsClientCodefile, code);

            return code;
        }
    }
}
