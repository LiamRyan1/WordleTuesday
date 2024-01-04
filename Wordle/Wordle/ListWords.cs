using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;


namespace Wordle
{
     
    public class ListWords
    {
        List<string> words = new List<string>();
        HttpClient httpClient;
        string savedfilelocation = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "words.txt");

        public async Task getWordList()
        {
            if(File.Exists(savedfilelocation))
            {
                ReadFileIntoList();
            }
            else
            {
               await DownloadFile();
                ReadFileIntoList();

            }
        }
        public void ReadFileIntoList()
        {
            StreamReader sr = new StreamReader(savedfilelocation);  
            string word = "";
            while((word = sr.ReadLine()) != null)
            {
                words.Add(word);
            }
            sr.Close();
        }
        public async Task DownloadFile()
        {

            using (var httpClient = new HttpClient())
            {
                var responseStream = await httpClient.GetStreamAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");
                using var fileStream = new FileStream(savedfilelocation, FileMode.Create);
                responseStream.CopyToAsync(fileStream);
            }
        }
        public String GenerateRandomWord()
        {
            Random random = new Random();
            int which = random.Next(words.Count);
            return words[which];
        }
    }
}
    

