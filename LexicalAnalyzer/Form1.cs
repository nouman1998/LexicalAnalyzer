using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace LexicalAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //textBox1.Text = "";
            Classification abc = new Classification();
            textBox1.Text = "\n" + (abc.isString(textBox1.Text)).ToString();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string a="";
            string[] abc = new string[1000];
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @"C:\Users\DevOps\Desktop\";
            openFileDialog1.Title = "Browse Source Files";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
             a = System.IO.File.ReadAllText(openFileDialog1.FileName);
            }
            textBox1.Text = a;
            //foreach (var item in abc)
            //{
            //    textBox1.AppendText(item);
            //}
        }
        public int  lineNumber = 1, wordNumber = 1;
        public string temp = "", testString;
        public int flag = 0;

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lineNumber = 1;
            breakword();
        }
        public void breakword()
        {
          
            Classification.tokenList.Clear();
            temp = "";
            string source = textBox1.Text;
            char c;
            bool isString = false, isComment = false, isChar = false;

            for (int i = 0; i < source.Length; i++)
            {
            start:;

                c = source[i];
                if (c == 32 && !isString)
                {
                    if (temp != "")
                    {
                          addToList(temp);
                    }
                    continue;
                }
                if (c == 10)
                {
                    lineNumber++;
                }
                if (c == 13 || c == 10)
                {
                    if (!isString || !isComment)
                    {
                        if (temp != "")
                        {
                              addToList(temp);

                        }
                        continue;
                    }
                }
                //comments
                if (!isString)
                {

                    if (c == '/')
                    {
                        if (source.Length != (i + 1))
                        {
                            testString = c.ToString() + source[i + 1].ToString();

                            if (testString == "//")
                            {
                                isComment = true;
                                if (temp != "")
                                {
                                    addToList(temp);
                                    //flag++;
                                }
                               


                                while (isComment)
                                {
                                    
                                    if (c == 10 || (i + 1) == source.Length)
                                    {
                                        isComment = false;
                                        i++;
                                        if (c == 10)
                                        {
                                            lineNumber++;
                                            goto start;

                                        }

                                        continue;

                                    }
                                  
                                    i++;
                                    c = source[i];
                                }

                            }
                           else if (testString == "/*")
                            {
                                isComment = true;
                                if (temp != "")
                                {
                                    addToList(temp);
                                    //flag++;
                                }
                                
                                while (isComment)
                                {
                                    //c = source[i];
                                    if (i + 1 == source.Length)
                                    {
                                        isComment = false;
                                        continue;
                                    }
                                    testString = c.ToString() + source[i + 1];
                                    if (testString == "*/")
                                    {
                                        i = i + 2;
                                            c = source[i];
                                        isComment = false;
                                        
                                        continue;
                                    }
                                    i++;
                                    c = source[i];
                                    if (c == 10)
                                    {
                                        lineNumber++;
                                    }
                                }



                            }


                        }

                        if (i == source.Length)
                        {
                            goto end;
                        }
                        else
                        {
                            goto start;
                        }

                    }
                   


                }
                if (temp == "")
                {
                    if (regexCheck(c, 3))//Punctuactors
                    {
                        addToList(c);
                    }
                    else if (regexCheck(c, 6))//compound operators
                    {
                        if (source.Length != (i + 1))
                        {
                            testString = c.ToString() + source[i + 1].ToString();
                            if (isCompound(testString))
                            {
                                i++;
                                addToList(testString);
                            }

                            else//checking simpple + - only
                            {
                                if (regexCheck(c, 5))
                                {
                                    temp += c;
                                }
                                else
                                {
                                    addToList(c);
                                }
                            }
                        }
                        else if (c != 10 || c != 13)
                        {
                            temp += c;
                        }
                    }
                    else
                    {
                        if (c == '"')
                        {
                            isString = true;
                        }
                        else if (c == '\'')
                        {
                            isChar = true;

                        }
                        temp += c;
                        if (isChar)
                        {
                            if (i + 1 != source.Length)
                            {
                                int charLength = 2;
                                if (source[i + 1] == '\\')
                                {
                                    charLength = 3;
                                }
                                while (charLength > 0)
                                {
                                    i++;
                                    c = source[i];
                                    temp += c;
                                    charLength--;   
                                }
                            }
                            addToList(temp);
                            isChar = false;
                            continue;

                        }
                       
                    }

                }
                else//temp is not empty
                {
                    if (isString)
                    {


                        if (c == 13||c==10)
                        {
                            isString = false;
                            addToList(temp);
                            continue;
                        }

                        temp += c;
                        if (c == '"')
                        {//
                            isString = false;
                            addToList(temp);
                        }
                    }
                    else if (regexCheck(c, 1))//agr c alphabet ha aur usse phle . 
                                              //hai to break
                    {
                        if (temp == "." || regexCheck(temp, 5))//+_
                        {
                            addToList(temp);
                        }
                        temp += c;

                    }
                    else if (regexCheck(c, 2))//c is number or not
                    {
                        temp += c;

                    }
                    else
                    {
                        if (c == '\'')
                        {
                              addToList(temp);
                            goto start;
                        }
                        else
                        {
                            if (c =='.')
                            {
                                if (regexCheck(temp,5))//numbers to nahi
                                {
                                    temp += c;
                                }
                                else//anything else than numbers
                                {
                                    addToList(temp);
                                    temp += c;
                                }
                            }////
                            else if (c == '\"')
                            {
                                if (temp != "")
                                {
                                    addToList(temp);
                                    temp += c;
                                    isString = true;

                                }
                              
                                

                            }
                            else if (regexCheck(c, 3))//punc
                            {
                                addToList(temp);
                                addToList(c);
                            }
                            else if (regexCheck(c, 6))
                            {
                                addToList(temp);
                                if (source.Length != (i + 1))
                                {
                                    testString = c.ToString() + source.ElementAt(i + 1);
                                    if (isCompound(testString))
                                    {
                                        i++;
                                        addToList(testString);
                                    }
                                    else
                                    {
                                        //+- check
                                        if (regexCheck(c, 5))
                                        {
                                            addToList(c);
                                            i++;
                                            c = source[i];
                                            goto start;
                                        }
                                        else
                                        {
                                            addToList(c);
                                        }
                                    }
                                }
                                else
                                {
                                    temp += c;
                                }
                            }
                           
                            else if (regexCheck(temp, 4))
                            {
                                addToList(temp);
                                temp += c;
                            }
                            else
                            {
                                temp += c;
                            }
                        }
                    }
                }

            end:;
            }
            if (temp != "")
            {
                addToList(temp);
            }

            writeToFile();
        }







        public bool regexCheck(dynamic keyword, int type)
        {
            string regex = "";
            switch (type)
            {
                //char is alphabet
                case 1:
                    regex = @"^[a-zA-Z]$";
                    break;
                //string is numbers 0-9
                case 2:
                    regex = @"^[0-9]+$";
                    break;
                //limited punctuators
                case 3:
                    regex = @"^[;\\,(){}[\]]$";
                    break;
                //check string if float
                case 4:
                    regex = @"^[+-]?[0-9]*\.?[0-9]*$";
                    break;
                //+- at starting and then numbers
                case 5:
                    regex = @"^[+-]?[0-9]*$";
                    break;
                //compound ops <>!=+-*/%&|
                case 6:
                    regex = @"^[<>!=\-+*/%&|]$";
                    break;
                default:
                    break;
            }
            return regexCheck(keyword.ToString(), regex);
        }
        public bool regexCheck(dynamic keyword, string regex)
        {
            Match m = Regex.Match(keyword.ToString(), regex);
            if (m.Success)
            {
                return true;
            }
            else
                return false;
        }
        public void addToList(dynamic value)
        {
            Classification.tokenList.Add(new Tokens("", value.ToString(),   lineNumber));
            wordNumber++;
            temp = "";
        }
        public bool isCompound(string s)
        {
            return Classification.compoundOperators.Contains(s);
        }



        char a= '\\';
        static void writeToFile()
        {
            //string[] arr = new string[Classification.tokenList.Count+3];
           
            string a="";
            string path = @"C:\Users\Nouman Ejaz\Desktop\tokens.txt";
            Classification c2 = new Classification();
            c2.switch_case();
            StreamWriter sw = new StreamWriter(path);
            foreach (var token in Classification.tokenList)
            {
                sw.WriteLine("Line Number:  {0}  Class Keyword:   {1}    Value Part: {2}", token.lineNumber, token.classKeyword, token.value);
                a += "Line Number: "+token.lineNumber +"   Class Keyword: "+ token.classKeyword + "  Value:    "+ token.value+ "\n \n";
             
            
            }
            sw.Close();
            System.Diagnostics.Process.Start("notepad", path);
         
           
           
       
            
            //MessageBox.Show("Compilation Completed"); 
        }



    }


     






}
        
    

