using SmartDesktopApp.Features.Group1;
using SmartDesktopApp.Features.Home;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace SmartDesktopApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // 1. 掃描有哪些語言檔案
            Core.LanguageManager.ScanLanguages();

            // 2. 取得使用者電腦的語系名稱 (例如 "zh-TW", "en-US", "ja-JP")
            string systemLang = System.Globalization.CultureInfo.CurrentCulture.Name;

            // 3. 決定要載入哪一個語系檔
            string finalLang = "en-US"; // 預設為英文 (Fallback)

            // 檢查我們有沒有對應系統語系的 JSON 檔 (例如有無 zh-TW.json)
            // 這裡我們比對 AvailableLanguages 字典裡的 Value (檔案名稱)
            foreach (var langCode in Core.LanguageManager.AvailableLanguages.Values)
            {
                if (langCode.Equals(systemLang, StringComparison.OrdinalIgnoreCase))
                {
                    finalLang = langCode; // 找到了完全匹配的語系
                    break;
                }
            }

            // 4. 執行載入與界面更新
            Core.LanguageManager.LoadLanguage(finalLang);
            RefreshUI();

            // 5. 填充 ComboBox 並將選單指到目前載入的語系
            FillLanguageComboBox(finalLang);

            ShowModule(new UC_Home());

            // 讓程式一打開，右邊就顯示首頁模組
            ShowModule(new UC_Home());
            lblStatus.Text = "● 系統就緒 | 請選擇功能模組";
        }
        // 這是一個萬用的方法，用來把學生的 UserControl 塞進主面板
        public void ShowModule(UserControl userControl)
        {
            // 1. 清除右側面板目前顯示的內容
            mainContentPanel.Controls.Clear();

            // 2. 設定學生的控制項填滿整個區域
            userControl.Dock = DockStyle.Fill;

            // 3. 將控制項加入面板
            mainContentPanel.Controls.Add(userControl);
        }

        private void btnModule1_Click(object sender, EventArgs e)
        {
            // 1. 執行切換模組的方法
            ShowModule(new UC_Group1());

            // 2. 同步更新狀態列文字
            lblStatus.Text = "● 目前正在執行：第一組:XXX";
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            ShowModule(new UC_Home());
            lblStatus.Text = "● 系統就緒 | 請選擇功能模組";
        }
        private void RefreshUI()
        {
            // 這裡就是「解耦」：不再直接打字，而是去查字典
            this.lblMainTitle.Text = Core.LanguageManager.GetString("SYS_TITLE");
            this.lblSubtitle.Text = Core.LanguageManager.GetString("SYS_SUBTITLE");
            this.btnHome.Text = Core.LanguageManager.GetString("BTN_HOME");
            this.btnModule2.Text = Core.LanguageManager.GetString("BTN_GROUP2");
            this.btnModule3.Text = Core.LanguageManager.GetString("BTN_GROUP3");
            this.btnModule4.Text = Core.LanguageManager.GetString("BTN_GROUP4");
            this.btnModule5.Text = Core.LanguageManager.GetString("BTN_GROUP5");
            this.btnModule6.Text = Core.LanguageManager.GetString("BTN_GROUP6");
            this.btnModule7.Text = Core.LanguageManager.GetString("BTN_GROUP7");
            this.btnModule8.Text = Core.LanguageManager.GetString("BTN_GROUP8");
            this.btnModule9.Text = Core.LanguageManager.GetString("BTN_GROUP9");
            this.btnModule10.Text = Core.LanguageManager.GetString("BTN_GROUP10");
            this.lblStatus.Text = Core.LanguageManager.GetString("LBL_STATUS_READY");
        }
        
        private void cboLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLanguages.SelectedItem == null) return;

            // 1. 取得選取的語言名稱 (例如 "English")
            string selectedName = cboLanguages.SelectedItem.ToString();

            // 2. 從字典找出對應的代碼 (例如 "en-US")
            string langCode = Core.LanguageManager.AvailableLanguages[selectedName];

            // 3. 載入語系檔案並刷新 UI
            Core.LanguageManager.LoadLanguage(langCode);
            RefreshUI();
        }
        private void FillLanguageComboBox(string currentLangCode)
        {
            cboLanguages.Items.Clear();
            int selectedIndex = 0;
            int count = 0;

            foreach (var entry in Core.LanguageManager.AvailableLanguages)
            {
                cboLanguages.Items.Add(entry.Key); // Key 是顯示名稱，如 "English"

                // 如果這個選項的代碼 (Value) 等於我們載入的代碼，就記住它的位置
                if (entry.Value.Equals(currentLangCode, StringComparison.OrdinalIgnoreCase))
                {
                    selectedIndex = count;
                }
                count++;
            }

            // 設定選單選取正確的語言
            if (cboLanguages.Items.Count > 0)
            {
                cboLanguages.SelectedIndex = selectedIndex;
            }
        }

        private void btnModule2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 取得當前組件的版本資訊
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string shortVersion = $"{version.Major}.{version.Minor}.{version.Build}";
            // 或者，如果您有設計一個 label 用來顯示版本：
            lblSubtitle.Text= $"大灣高中 × 沙崙高中 聯名專案 v{shortVersion}" ;
        }
    }
}
