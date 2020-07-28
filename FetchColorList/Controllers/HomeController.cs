using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FetchColorList.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FetchColorList.Controllers
{
    public class HomeController : Controller
    {
        static Root deserialzedObject;
        static List<ColorData> ListOfColors;
        List<ColorData> Group1 = new List<ColorData>();
        List<ColorData> Group2 = new List<ColorData>();
        List<ColorData> Group3 = new List<ColorData>();


        public async Task<IActionResult> Index()
        {
            await CreateListOfColors();
            ListOfColors = ListOfColors.OrderBy(x => x.year).ToList();
            SortListIntoThreeGroups();
            ViewBag.Group1 = Group1;
            ViewBag.Group2 = Group2;
            ViewBag.Group3 = Group3;
            return View();
        }

        private async Task CreateListOfColors()
        {
            ListOfColors = new List<ColorData>();
            await getPage(1);
            int pages = deserialzedObject.total_pages;
            if (pages > 1)
            {
                for (int i = 2; i <= pages; i++)
                {
                    await getPage(i);
                }
            }
        }

        private async Task getPage(int pageNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage message = await client.GetAsync("https://reqres.in/api/example?per_page=2&page=" + pageNumber))
                {
                    var result = message.Content.ReadAsStringAsync().Result;
                    if (message.IsSuccessStatusCode)
                    {
                        deserialzedObject = JsonConvert.DeserializeObject<Root>(result);
                        ListOfColors = ListOfColors.Concat(deserialzedObject.data).ToList();
                    }
                }
            }
        }

        private void SortListIntoThreeGroups()
        {
            foreach (ColorData ColorObj in ListOfColors)
            {
                int rest;
                string firstValueInpantone = null;
                string[] pantoneValues = ColorObj.pantone_value.Split('-');
                if (pantoneValues.Length >= 1)
                {
                    firstValueInpantone = pantoneValues[0];
                }
                if (Int32.TryParse(firstValueInpantone, out rest))
                {
                    if ((rest % 3) == 0)
                    {
                        Group1.Add(ColorObj);
                    }
                    else if ((rest % 2) == 0)
                    {
                        Group2.Add(ColorObj);
                    }
                    else
                    {
                        Group3.Add(ColorObj);
                    }
                }
            }
        }
    }
}
