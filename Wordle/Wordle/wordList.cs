using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Wordle
{
  public class wordList
   {
        List<word> WordList;
        HttpClient httpClient;


        public wordList()
        {
            httpClient = new HttpClient();
            WordList = new();
           // GetwordListCommand = new Command(async () => await MakeCollection());
        }
        private async Task getWordList()
        {
            if (WordList.Count > 0)
            {
                return;
            }
            var response = await httpClient.GetAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");
            if(response.IsSuccessStatusCode)
            {
                string contents = await response.Content.ReadAsStringAsync();
                WordList = JsonSerializer.Deserialize<List<word>>(contents);
            }
        }
    }
}
