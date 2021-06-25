using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AITalk;


namespace QuizHint
{
    class talk
    {
        protected AITalkUtil aitalk; //ラッパークラスのサンプル

        // write your AITalk dir ==> AITALKDIR
        // fill licenskey you have ==> licensekey
        private const string licensepath = "AITALKDIR\\license\\aitalk_win.lic";
        private const string licensekey = "********************";
        private const string voicepath = "AITALKDIR\\dic\\voice";
        private const string langpath = "AITALKDIR\\dic\\lang";
        private string speaker = "kaho_22";
        private int samplingrate = 22050;
        private uint timeout = 1000;
        private const string worddicpath = "word.wdic";
        private const string phrasedicpath = "phrase.pdic";
        private const string pausedicpath = "symbol.sdic";
        private uint Latency = 100;
        private uint BufferSize = 176400;
        private string[][] kana;

        public talk()
        {
            aitalk = AITalkUtil.GetInstance();
            AITalkInit();
        }
        public talk(string[][] str)
        {
            aitalk = AITalkUtil.GetInstance();
            AITalkInit();
            kana = new string[str.Length][];
            TextToKana(str);
        }
        private void AITalkInit()
        {
            Init();                // AITalkAPI_Init
            LangLoad();            // AITalkAPI_LangLoad
            VoiceLoad();           // AITalkAPI_VoiceLoad
            LoadWordDic();       // AITalkAPI_ReloadWordDic
            LoadPhraseDic();     // AITalkAPI_ReloadPhraseDic
            LoadSymbolDic();     // AITalkAPI_ReloadSymbolDic
            OpenAudio();           // AIAudioAPI_Open
        }
        #region Initialize
        private void Init()
        {
            AITalk_TConfig config = new AITalk_TConfig();
            // サンプルレート
            try
            {
                config.hzVoiceDB = samplingrate;
            }
            catch (FormatException ex)
            {
                return;
            }
            // dbsパス
            config.dirVoiceDBS = voicepath;
            // タイムアウト
            try
            {
                config.msecTimeout = timeout;
            }
            catch (FormatException ex)
            {
                return;
            }
            config.pathLicense = licensepath;
            config.codeAuthSeed = licensekey;
            config.lenAuthSeed = 0;

            // AITalkエンジンを初期化する
            AITalkResultCode res = aitalk.Init(config);
        }
        private void LangLoad()
        {
            // 辞書ロード
            AITalkResultCode res = aitalk.LangLoad(langpath);
        }
        private void VoiceLoad()
        {
            // 音声ロード
            AITalkResultCode res = aitalk.VoiceLoad(speaker);
        }
        private void LoadWordDic()
        {
            // 単語辞書のロード
            AITalkResultCode res = aitalk.ReloadWordDic(worddicpath);
        }
        private void LoadPhraseDic()
        {
            // フレーズ辞書のロード
            AITalkResultCode res = aitalk.ReloadPhraseDic(phrasedicpath);
        }
        private void LoadSymbolDic()
        {
            // 記号ポーズ辞書のロード
            AITalkResultCode res = aitalk.ReloadSymbolDic(pausedicpath);
        }
        private void OpenAudio()
        {
            //音声デバイス
            AIAudio_TConfig config = new AIAudio_TConfig();
            // レイテンシ
            try
            {
                config.msecLatency = Latency;
            }
            catch (FormatException ex)
            {
                return;
            }
            // バッファサイズ
            try
            {
                config.lenBufferBytes = BufferSize;
            }
            catch (FormatException ex)
            {
                return;
            }
            // サンプルレート
            try
            {
                config.hzSamplesPerSec = samplingrate;
            }
            catch (FormatException ex)
            {
                return;
            }
            // フォーマットは 16bit Linear PCM 固定
            config.formatTag = AIAudioFormatType.AIAUDIOTYPE_PCM_16;
            config.__reserved__ = 0;

            // 音声デバイスを初期化する
            AIAudioResultCode res = aitalk.OpenAudio(ref config);
        }
        #endregion

        private void TextToKana(string[][] str)
        {
            int n = 0;
            foreach(string[] line in str)
            {
                int w = 0;
                kana[n] = new string[line.Length];
                foreach(string word in line)
                {
                    AITalkResultCode res = aitalk.TextToKana(word, out kana[n][w]);
                    w++;
                }
                n++;
            }
        }

        #region public function
        public void talkfromstring(string _text)
        {
            try
            {
                AITalkResultCode res = aitalk.TextToSpeech(_text);
            }
            catch
            {
            }
        }
        public void talkfromkana(int n, int w)
        {
            try
            {
                AITalkResultCode res = aitalk.KanaToSpeech(kana[n][w]);

            }
            catch
            {
            }
        }
        public void End()
        {
            AITalkResultCode res = aitalk.End();
        }
        #endregion
    }
}
