using System;
using System.Reflection.Emit;
using System.Xml.XPath;

namespace Babel
{    
    class Write_Passage
    {
        public static string Construct(int length,bool head=false)//构造一个单词，length为单词长度，head为单词首字母是否大写。
        {
            string Alphabet="aeiouybcdfghjklmnpqrstvwxz";//前六个为元音字母
            string result="";
            bool flag=false;
            Random ran = new Random();
            for(int i=1;i<=length;i++)
            {
                int n = ran.Next(0,25);
                if(head==true && i==1)
                    result+=Alphabet.ToUpper()[n];
                else
                    result+=Alphabet[n];
                if(n<=5) flag=true;//判断是否有元音
            }
            if(flag!=true)
            {
                int n = ran.Next(0,length-1);
                if(n!=0 || head==false)//需大写情况
                    result = result.Substring(0,n)+Alphabet[ran.Next(0,4)]+result.Substring(n+1);
                else
                    result = result.Substring(0,n)+Alphabet.ToUpper()[ran.Next(0,4)]+result.Substring(n+1);
            }
            return result;
        }
        public static int Length_Random()//长度随机函数
        {
            Random ran = new Random();
            int length=ran.Next(1,11);
            if(length==1)
                return ran.Next(1,3);
            else if(length<=8)
                return ran.Next(3,10);
            else
                return ran.Next(10,21);//这里很奇怪的10的概率格外的高
        }
        public static void Analysis()//分析函数
        {
            string passage=File.ReadAllText("test.txt");
            bool title_flag=true;
            string title="";
            int word_count=0, sent_count=0, para_count=0, eva_count=0;
            int[] word_freq = new int[25];
            string word="";
            for(int i=0;i<passage.Length;i++)
            {
                if(passage[i]==' ' && title_flag == true)
                {
                    title+=word+' ';
                    word="";
                }
                else if(passage[i]=='\n' && title_flag == true)//标题结束判断
                {
                    title+=word;
                    title_flag=false;
                    word="";
                }
                else if(passage[i] == ',' || passage[i] == ' ' && word.Trim() != string.Empty)
                {
                    word_count+=1;
                    word_freq[word.Length]+=1;
                    word="";
                }
                else if(passage[i] == '.')
                {
                    word_count+=1;
                    word_freq[word.Length]+=1;
                    sent_count+=1;
                    word="";
                }
                else if(passage[i] == '\n')
                {
                    para_count+=1;
                    word="";
                }
                else
                {
                    word+=passage[i];
                    if(word.Trim().ToLower()=="eva")
                        eva_count+=1;
                }
            }
            StreamWriter sw = new StreamWriter("analysis.txt");
            sw.Write("Title: "+title+'\n');
            sw.Write("Word Count: "+word_count.ToString()+'\n');
            sw.Write("Para Count: "+para_count.ToString()+'\n');
            sw.Write("Eva Appear: "+eva_count.ToString()+'\n');
            sw.Write("Word Freq:\n");
            for(int i=1;i<=20;i++)
                sw.Write(i.ToString()+". "+word_freq[i].ToString()+'\n');
            sw.Close();
        }
        static void Main(string[] args)//主函数
        {
            Random ran = new Random();
            string passage="";
            string title="";
            int passage_length=0;
            //标题构造
            int title_length=ran.Next(1,5);
            for(int i=1;i<=title_length;i++)
                title+=Construct(Length_Random(),true)+' ';
            //文章构造
            while(passage_length<=550)
            {
                while(passage_length<=550)
                {
                    int sentence_length=Length_Random();
                    string sentence="";
                    for(int i=1;i<=sentence_length;i++)
                    {
                        sentence+=Construct(Length_Random(),i==1?true:false);
                        if(ran.Next(1,10)==1 && i!=sentence_length)
                            sentence+=", ";
                        else if(i==sentence_length)
                            sentence+=". ";
                        else
                            sentence+=' ';
                    }
                    passage_length+=sentence_length;
                    passage+=sentence;
                    if(ran.Next(1,10)==1)
                    {
                        passage+='\n';
                        break;
                    }
                }
                passage+="    ";
            }
            File.WriteAllText("test.txt",title+'\n'+passage);
            Analysis();
        }
    }
}