using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms; 

namespace SmartDesktopApp.Core
{
    public static class LanguageManager
    {
        // 這裡一定要宣告 _dictionary 變數，後面的方法才找得到它
        private static Dictionary<string, string> _dictionary;

        // 用來存【顯示名稱】對應到【檔案代碼】的字典
        // 例如: "English" -> "en-US"
        public static Dictionary<string, string> AvailableLanguages = new Dictionary<string, string>();

        // 掃描資料夾內所有的 JSON 檔
        public static void ScanLanguages()
        {
            AvailableLanguages.Clear();
            // 關鍵修改：路徑指向執行檔目錄下的 "Core" 資料夾
            string corePath = Path.Combine(Application.StartupPath, "Core");
            if (Directory.Exists(corePath))
            {
                string[] files = Directory.GetFiles(corePath, "*.json");
                foreach (string file in files)
                {
                    try
                    {
                        string langCode = Path.GetFileNameWithoutExtension(file);
                        string json = File.ReadAllText(file);
                        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                        if (data != null && data.ContainsKey("LANG_NAME"))
                        {
                            AvailableLanguages.Add(data["LANG_NAME"], langCode);
                        }
                    }
                    catch { /* 忽略格式錯誤的 JSON */ }
                }
            }
        }

            // 讀取 JSON 檔案的方法
            public static void LoadLanguage(string langCode)
        {
            // 關鍵修改：路徑指向執行檔目錄下的 "Core" 資料夾
            string filePath = Path.Combine(Application.StartupPath, "Core", $"{langCode}.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                _dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
        }

        // 根據 Key 取得文字的方法 (這就是影片中的唯一識別碼)
        public static string GetString(string key)
        {
            if (_dictionary != null && _dictionary.ContainsKey(key))
                return _dictionary[key];
            return key; // 如果找不到，就回傳 Key 當作警告
        }
    }
}
