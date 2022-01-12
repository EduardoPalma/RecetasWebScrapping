using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace RecetasWebScrapping
{
    class Program
    {
        public static string RemoveAccentsWithNormalization(string inputString)
        {
            string normalizedString = inputString.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < normalizedString.Length; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalizedString[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
        static string nuevaPalabra(string palabra)
        {
            string nPalabra = "";
            for (int i = 0; i < palabra.Length; i++)
            {
                if (palabra[i] == ' ')
                {
                    nPalabra += "-";
                }
                else
                {
                    nPalabra += palabra[i];
                }

            }
            return nPalabra;
        }

        static string UrlMalas(string urlRecetas)
        {
            if (urlRecetas.Equals("https://www.gourmet.cl/recetas/champinones-a-lo-&#8220;empanada-de-pino&#8221;/")) return "https://www.gourmet.cl/recetas/champinones-rellenos-lo-empanada-pino/";
            if (urlRecetas.Equals("https://www.gourmet.cl/recetas/choclos-a-la-parrilla-con-soya-y-aji/")) return "https://www.gourmet.cl/recetas/choclos-la-parrilla-soya-aji-amarillo-sesamo/";
            if (urlRecetas.Equals("https://www.gourmet.cl/recetas/sopa-toscana-de-porotos/")) return "https://www.gourmet.cl/recetas/sopa-toscana-porotos/";
            if (urlRecetas.Equals("https://www.gourmet.cl/recetas/zapallo-spaghetti-con-tomate-cherry-y-albahaca/")) return "https://www.gourmet.cl/recetas/zapallo-spaghetti-tomate-cherry-albahaca/";
            if (urlRecetas.Equals("https://www.gourmet.cl/recetas/queque-de-zanahoria-vegano/")) return "https://www.gourmet.cl/recetas/queque-zanahoria-vegano/";
            if (urlRecetas.Equals("https://www.gourmet.cl/recetas/pan-arabe-plano/")) return "https://www.gourmet.cl/recetas/pan-plano-arabe/";
            if (urlRecetas.Equals("https://www.gourmet.cl/recetas/garbanzos/")) return "https://www.gourmet.cl/recetas/sopa-de-garbanzos/";
            if (urlRecetas.Equals("https://www.gourmet.cl/recetas/ceviche-de-champinones/")) return "https://www.gourmet.cl/recetas/cebiche-de-champinones/";
            return urlRecetas;
        }

        static void RecetasKeto()
        {
            HtmlWeb oWeb = new HtmlWeb();
            string[] urlRecetas = { "https://www.dietdoctor.com/es/keto/recetas-cetogenicas/comidas?s=&st=recipe&lowcarb%5B%5D=keto",
                                    "https://www.dietdoctor.com/es/keto/recetas-cetogenicas/comidas?s=&st=recipe&lowcarb%5B%5D=keto&sp=2",
                                    "https://www.dietdoctor.com/es/keto/recetas-cetogenicas/comidas?s=&st=recipe&lowcarb%5B%5D=keto&sp=3"};

            List<string> urlRecetasIngredientes = new List<string>();

            StreamReader archivoUrlRecetas = new StreamReader("C:/Users/Hello/Desktop/PA/UrlRecetas.txt");
            //lectura de url para cada receta
            while (!archivoUrlRecetas.EndOfStream)
            {
                string linea = archivoUrlRecetas.ReadLine();
                urlRecetasIngredientes.Add(linea);
            }
            archivoUrlRecetas.Close();

            TextWriter archivoRecetaKeto = new StreamWriter("C:/Users/Hello/Desktop/DatosProgramacionAvanzada/RecetasKeto.txt");
            
            int cant = 0;
            int numero = 97;
            foreach(var nomUrl in urlRecetas)
            {
                HtmlDocument doc = oWeb.Load(nomUrl);

                foreach (var nombreReceta in doc.DocumentNode.CssSelect(".preview-item-title"))
                {
                    string texto = nombreReceta.InnerHtml;
                    archivoRecetaKeto.WriteLine(numero + ";"+texto + ";DietDoctor;" + urlRecetasIngredientes[cant]);

                    TextWriter archivoIngredientesKeto = new StreamWriter("C:/Users/Hello/Desktop/DatosProgramacionAvanzada/IngredientesCategoriaKeto/ingredientesReceta ("+numero+").txt");
                    TextWriter archivoPreparacionKeto = new StreamWriter("C:/Users/Hello/Desktop/DatosProgramacionAvanzada/PreparacionCategoriaKeto/preparacionReceta (" + numero + ").txt");
                    HtmlDocument doc1 = oWeb.Load(urlRecetasIngredientes[cant]);
                    Console.WriteLine(texto);
                    foreach (var ingredientes in doc1.DocumentNode.CssSelect(".recipe-ingredients-list-wrapper"))
                    {
                        foreach(var ingredientesMayores in ingredientes.CssSelect("li"))
                        {
                            string ingre = ingredientesMayores.InnerText;                        
                            archivoIngredientesKeto.WriteLine(ingre);
                        }
                    }

                    foreach (var prepraraciones in doc1.DocumentNode.CssSelect(".recipe-steps-list").CssSelect(".recipe-steps-item"))
                    {
                        string prepa = prepraraciones.InnerText;
                        archivoPreparacionKeto.WriteLine(prepa);
                    }
                    archivoPreparacionKeto.Close();
                    archivoIngredientesKeto.Close();
                    cant++;
                    numero++;
                }
            }
            archivoRecetaKeto.Close();
            Console.WriteLine(cant);
        }
        static void RecetasVegana()
        {
            HtmlWeb oWeb = new HtmlWeb();
            string url = "https://www.gourmet.cl/tipo-plato/vegano/";
            string urlReceta = "https://www.gourmet.cl/recetas/";
            HtmlDocument doc = oWeb.Load(url);


            TextWriter archivo = new StreamWriter("C:/Users/Hello/Desktop/DatosProgramacionAvanzada/RecetasVegana.txt");
            
            int cant = 34;
            foreach (var nombreReceta in doc.DocumentNode.CssSelect("h2"))
            {
                string texto = nombreReceta.InnerHtml;
                string textoNormalizado = nuevaPalabra(RemoveAccentsWithNormalization(texto.ToLower()));
                urlReceta += textoNormalizado + "/";
                Console.WriteLine(texto);

                urlReceta = UrlMalas(urlReceta);
                archivo.WriteLine(cant+";"+texto+";gourmet;"+urlReceta+";");

                TextWriter archivoIngredientes = new StreamWriter("C:/Users/Hello/Desktop/DatosProgramacionAvanzada/IngredientesCategoriaVeganas/ingredientesReceta " + "("+cant+").txt");
                TextWriter archivoPrepraracion = new StreamWriter("C:/Users/Hello/Desktop/DatosProgramacionAvanzada/PreparacionCategoriaVeganas/preparacionReceta " + "(" + cant + ").txt");

                HtmlDocument doc1 = oWeb.Load(urlReceta);
                foreach (var ingredientes in doc1.DocumentNode.CssSelect("#contenedor-imprimir").First().CssSelect("ul"))
                {
                    foreach(var ingredientesEspecifico in ingredientes.CssSelect("li"))
                    {
                        string text = ingredientesEspecifico.InnerHtml;
                        archivoIngredientes.WriteLine(text);
                        //Console.WriteLine(text);
                    }
              
                }

            
                foreach (var preparacion in doc1.DocumentNode.CssSelect("#contenedor-imprimir").First().CssSelect("ol"))
                {
                    foreach (var preparacionEspecifica in preparacion.CssSelect("li"))
                    {
                        string text = preparacionEspecifica.InnerHtml;
                        archivoPrepraracion.WriteLine(text);
                        //Console.WriteLine(text);
                    }

                }
                urlReceta = "https://www.gourmet.cl/recetas/";
                cant++;
                archivoIngredientes.Close();
                archivoPrepraracion.Close();
            }
            
            
            url = "https://www.gourmet.cl/tipo-plato/vegano/page/2/";
            doc = oWeb.Load(url);
            foreach (var nombreReceta in doc.DocumentNode.CssSelect("h2"))
            {
                string texto = nombreReceta.InnerHtml;
                string textoNormalizado = nuevaPalabra(RemoveAccentsWithNormalization(texto.ToLower()));
                urlReceta += textoNormalizado + "/";
                urlReceta = UrlMalas(urlReceta);
                Console.WriteLine(texto);
                
                archivo.WriteLine(cant + ";" + texto + ";gourmet;" + urlReceta + ";");

                TextWriter archivoIngredientes = new StreamWriter("C:/Users/Hello/Desktop/DatosProgramacionAvanzada/IngredientesCategoriaVeganas/ingredientesReceta " + "(" + cant + ").txt");
                TextWriter archivoPrepraracion = new StreamWriter("C:/Users/Hello/Desktop/DatosProgramacionAvanzada/PreparacionCategoriaVeganas/preparacionReceta " + "(" + cant + ").txt");

                HtmlDocument doc1 = oWeb.Load(urlReceta);

                foreach (var ingredientes in doc1.DocumentNode.CssSelect("#contenedor-imprimir").First().CssSelect("ul"))
                {
                    foreach (var ingredientesEspecifico in ingredientes.CssSelect("li"))
                    {
                        string text = ingredientesEspecifico.InnerHtml;
                        archivoIngredientes.WriteLine(text);
                        //Console.WriteLine(text);
                    }

                }

                foreach (var preparacion in doc1.DocumentNode.CssSelect("#contenedor-imprimir").First().CssSelect("ol"))
                {
                    foreach (var preparacionEspecifica in preparacion.CssSelect("li"))
                    {
                        string text = preparacionEspecifica.InnerHtml;
                        archivoPrepraracion.WriteLine(text);
                        //Console.WriteLine(text);
                    }

                }
                urlReceta = "https://www.gourmet.cl/recetas/";
                cant++;
                archivoIngredientes.Close();
                archivoPrepraracion.Close();
            }
            archivo.Close();

        }

        static void crearArchivos()
        {
            for(int i= 65;i < 92; i++)
            {
                TextWriter archivoRecetaKeto = new StreamWriter("C:/Users/Hello/Desktop/DatosProgramacionAvanzada/IngredientesParceadosVegetarianas/ingredientesParceados (" + i + ").txt");
                archivoRecetaKeto.Close();
            }
        }
        static void Main(string[] args)
        {
            crearArchivos();
        }
    }
}
