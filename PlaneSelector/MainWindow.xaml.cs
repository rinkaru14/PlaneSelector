using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading;
using System.Windows.Threading;

namespace PlaneSelector
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {

        public static BitmapImage ToWpfBitmapImage(System.Drawing.Image image)
        {
            if (image == null)
                return null;

            var bimg = new BitmapImage();

            var ms = new MemoryStream();

            image.Save(ms, image.RawFormat);

            ms.Seek(0, SeekOrigin.Begin);

            bimg.BeginInit();

            bimg.StreamSource = ms;

            bimg.EndInit();

            return bimg;
        }

        InlineUIContainer Createicon(String name ,int fontsize)
        {
            string mypath = (@"..\..\Icons\" + name + ".png");

            BitmapImage tempimg = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"..\..\..\Icons\" + name + ".png", UriKind.Absolute));
            InlineUIContainer tempicon = new InlineUIContainer(new Image { Source = tempimg, Width = tempimg.PixelWidth*fontsize/32, Height = tempimg.PixelHeight * fontsize / 32, VerticalAlignment = VerticalAlignment.Center });
            tempicon.BaselineAlignment = BaselineAlignment.Center;
            return tempicon;
        }

        Brush Colorfeed(String cardcolor)
        {
            if (cardcolor == "W") return Brushes.LightYellow;
            else if (cardcolor == "R") return Brushes.MistyRose;
            else if (cardcolor == "U") return Brushes.LightBlue;
            else if (cardcolor == "G") return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC0FBC0"));
            else if (cardcolor == "B") return Brushes.LavenderBlush;
            else if (cardcolor == "C") return Brushes.GhostWhite;
            else if (cardcolor == "UB") return new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFADD8E6"), (Color)ColorConverter.ConvertFromString("#FFFFF0F5"), 0);
            else if (cardcolor == "UR") return new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFADD8E6"), (Color)ColorConverter.ConvertFromString("#FFFFE4E1"), 0);
            else if (cardcolor == "WRU")
            {
                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFFFFFE0"), (Color)ColorConverter.ConvertFromString("#FFADD8E6"), 0);
                myLinearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFFFE4E1"), 0.5f));
                return myLinearGradientBrush;
            }
            else return Brushes.Transparent;
        }

        Int32 typeboxsize(String type)
        {
            switch (type)
            {
                
                case "타마":
                case "움르":
                case "나나시":
                case "레이라":
                case "구즈코":
                    return 72;
                case "피루루크":
                case "리멤버":
                case "타윌":
                case "미도리코":
                    return 96;
                case "니지산지":
                    return 124;
                default:
                    return 0;
            }
        }

        Int32 limitationboxsize(String limitation)
        {
            switch (limitation)
            {
                case "에마 한정":
                    return 104;
                case "움르 한정":
                case "나나시 한정":
                case "레이라 한정":
                case "구즈코 한정":
                    return 122;
                case "타윌 한정":
                    return 140;
                case "피루루크 한정":
                case "미도리코 한정":
                    return 148;
                case "니지산지 한정":
                    return 172;
                default:
                    return 0;
            }
        }

        Int32 classboxsize(String signiclass)
        {
            switch (signiclass)
            {
                case "정원":
                    return 74;
                case "정계:미궁":
                case "정라:식물":
                case "정상:악마":
                case "정생:괴이":
                case "정계:승기":
                case "정계:전기":
                    return 114;
                case "정무:웨폰":
                case "정무:트릭":
                    return 156;
                default:
                    return 0;
            }
        }

        void wordadd(String word, Paragraph paragraph, int fontsize)
        {
            if (word == "[상시]") paragraph.Inlines.Add(Createicon("constant", fontsize));
            else if (word == "[드라이브 상시]") paragraph.Inlines.Add(Createicon("drive_constant", fontsize));
            else if (word == "[드라이브 자동]") paragraph.Inlines.Add(Createicon("drive_auto", fontsize));
            else if (word == "[출현]") paragraph.Inlines.Add(Createicon("on_play", fontsize));
            else if (word == "[자동]") paragraph.Inlines.Add(Createicon("auto", fontsize));
            else if (word == "[기동]") paragraph.Inlines.Add(Createicon("action", fontsize));
            else if (word == "[턴 1]") paragraph.Inlines.Add(Createicon("turn1", fontsize));
            else if (word == "[턴 2]") paragraph.Inlines.Add(Createicon("turn2", fontsize));
            else if (word == "[비트]") paragraph.Inlines.Add(Createicon("beat", fontsize));
            else if (word == "[게임 1]") paragraph.Inlines.Add(Createicon("game1", fontsize));
            else if (word == "[라이드]") paragraph.Inlines.Add(Createicon("ride", fontsize));
            else if (word == "[라이즈조건]") paragraph.Inlines.Add(Createicon("risecondition", fontsize));
            else if (word == "[라이즈]") paragraph.Inlines.Add(Createicon("rise", fontsize));
            else if (word == "[메인페이즈]") paragraph.Inlines.Add(Createicon("mainphase", fontsize));
            else if (word == "[어택페이즈]") paragraph.Inlines.Add(Createicon("attackphase", fontsize));
            else if (word == "(Coin)") paragraph.Inlines.Add(Createicon("coin", fontsize));
            else if (word == "(Down)") paragraph.Inlines.Add(Createicon("down", fontsize));
            else if (word == "(W)") paragraph.Inlines.Add(Createicon("ena_w", fontsize));
            else if (word == "(W0)") paragraph.Inlines.Add(Createicon("ena_w0", fontsize));
            else if (word == "(W2)") paragraph.Inlines.Add(Createicon("ena_w2", fontsize));
            else if (word == "(R)") paragraph.Inlines.Add(Createicon("ena_r", fontsize));
            else if (word == "(R0)") paragraph.Inlines.Add(Createicon("ena_r0", fontsize));
            else if (word == "(R2)") paragraph.Inlines.Add(Createicon("ena_r2", fontsize));
            else if (word == "(U)") paragraph.Inlines.Add(Createicon("ena_u", fontsize));
            else if (word == "(U0)") paragraph.Inlines.Add(Createicon("ena_u0", fontsize));
            else if (word == "(U1)") paragraph.Inlines.Add(Createicon("ena_u1", fontsize));
            else if (word == "(U2)") paragraph.Inlines.Add(Createicon("ena_u2", fontsize));
            else if (word == "(G)") paragraph.Inlines.Add(Createicon("ena_g", fontsize));
            else if (word == "(G0)") paragraph.Inlines.Add(Createicon("ena_g0", fontsize));
            else if (word == "(G1)") paragraph.Inlines.Add(Createicon("ena_g1", fontsize));
            else if (word == "(B)") paragraph.Inlines.Add(Createicon("ena_b", fontsize));
            else if (word == "(B2)") paragraph.Inlines.Add(Createicon("ena_b2", fontsize));
            else if (word == "(C)") paragraph.Inlines.Add(Createicon("ena_c", fontsize));
            else if (word == "[위공백]")
            {
                InlineUIContainer upspacing = Createicon("spacing", fontsize);
                upspacing.BaselineAlignment = BaselineAlignment.Bottom;
                paragraph.Inlines.Add(upspacing);
            }
            else if (word == "[아래공백]")
            {
                InlineUIContainer bottomspacing = Createicon("spacing", fontsize);
                bottomspacing.BaselineAlignment = BaselineAlignment.Top;
                paragraph.Inlines.Add(bottomspacing);
            }
            else if (word == "[라이프버스트]") paragraph.Inlines.Add(Createicon("lifeburst", fontsize));
            else paragraph.Inlines.Add(word);
        }

        public MainWindow()
        {

            string packindex = "WDK03";
            string packst = File.ReadAllText(@"..\..\Texts\" + packindex + ".json");
            JArray pack = JArray.Parse(packst);

            foreach (JObject card in pack)
            {
                String no = card["no"].ToObject<String>();
                String name = card["name"].ToObject<String>();
                String category = card["category"].ToObject<String>();
                String imgurl;
                if ((card["imgurl"] != null)) imgurl = card["imgurl"].ToObject<String>();
                else imgurl = "https://www.takaratomy.co.jp/products/wixoss/wxwp/images/card/" + packindex + "/" + no + ".jpg";
                String cardcolor = card["color"].ToObject<String>();

                bool vertical = (category != "키");
                double width = vertical ? 1000 : 1396;
                double height = vertical ? 1396 : 1000;
                Canvas canvas = new Canvas
                {
                    Background = Brushes.Transparent,
                    Width = width,
                    Height = height,
                };

                if (category == "루리그")
                {
                    String type = card["type"].ToObject<String>();


                    TextBlock Lrig_index = new TextBlock
                    {
                        Text = "루리그",
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 76,
                        Height = 24,
                        FontSize = 18,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetTop(Lrig_index, 36);
                    Canvas.SetLeft(Lrig_index, 40);
                    canvas.Children.Add(Lrig_index);

                    TextBlock Lrig_name = new TextBlock
                    {
                        Text = name,
                        Background = Colorfeed(cardcolor),
                        Foreground = Brushes.Black,
                        TextWrapping = TextWrapping.NoWrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 728,
                        Height = 68,
                        FontSize = (card["namesize"] != null) ? card["namesize"].ToObject<int>() : 50,
                        FontWeight = FontWeights.Bold
                    };


                    Canvas.SetTop(Lrig_name, 68);
                    Canvas.SetLeft(Lrig_name, 136);
                    canvas.Children.Add(Lrig_name);

                    TextBlock Lrig_limit = new TextBlock
                    {
                        Text = "리미트",
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 76,
                        Height = 24,
                        FontSize = 18,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetTop(Lrig_limit, 244);
                    Canvas.SetLeft(Lrig_limit, 40);
                    canvas.Children.Add(Lrig_limit);

                    TextBlock Lrig_type = new TextBlock
                    {
                        Text = type,
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Width = typeboxsize(type),
                        Height = 32,
                        FontSize = 22,
                        FontWeight = FontWeights.Bold,
                    };
                    Canvas.SetTop(Lrig_type, 928);
                    Canvas.SetRight(Lrig_type, 68);
                    canvas.Children.Add(Lrig_type);

                    if ((card["effect"] != null))
                    {
                        RichTextBox Lrig_text = new RichTextBox
                        {
                            Background = Colorfeed(cardcolor),
                            Foreground = Brushes.Black,
                            VerticalContentAlignment = VerticalAlignment.Top,
                            Width = 816,
                            Height = 244,
                            FontSize = (card["textsize"] != null) ? card["textsize"].ToObject<int>() : 27,
                            Padding = new Thickness(0, 5, 0, 5),
                            FontWeight = FontWeights.DemiBold,
                            IsReadOnly = true,
                            BorderThickness = new Thickness(0)
                        };

                        Paragraph paragraph = new Paragraph();

                        foreach (JArray line in card["effect"] as JArray)
                        {
                            foreach (var wordtoken in line)
                            {
                                String word = wordtoken.ToObject<String>();
                                wordadd(word, paragraph, (int)Lrig_text.FontSize);
                            }
                            if (line != card["effect"].Last) paragraph.Inlines.Add("\n");
                        }
                        Lrig_text.Document.Blocks.Clear();
                        Lrig_text.Document.Blocks.Add(paragraph);

                        Canvas.SetTop(Lrig_text, 1000);
                        Canvas.SetLeft(Lrig_text, 92);
                        canvas.Children.Add(Lrig_text);
                    }

                    if ((card["coin"] != null))
                    {
                        int coin = card["coin"].ToObject<int>();
                        TextBlock Lrig_coin = new TextBlock
                        {
                            Text = "코인",
                            Background = Brushes.Black,
                            Foreground = Brushes.White,
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center,
                            Width = 56,
                            Height = 20,
                            FontSize = 15,
                            FontWeight = FontWeights.Bold,
                        };
                        Canvas.SetTop(Lrig_coin, 1220);
                        Canvas.SetLeft(Lrig_coin, 888);
                        canvas.Children.Add(Lrig_coin);
                    }

                    TextBlock Lrig_cost = new TextBlock
                    {
                        Text = "그로우",
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 68,
                        Height = 20,
                        FontSize = 15,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetTop(Lrig_cost, 1240);
                    Canvas.SetLeft(Lrig_cost, 48);
                    canvas.Children.Add(Lrig_cost);
                }

                if (category == "아츠")
                {
                    JArray timings = card["timing"] as JArray;

                    TextBlock Arts_index = new TextBlock
                    {
                        Text = "아츠",
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 76,
                        Height = 24,
                        FontSize = 18,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetTop(Arts_index, 36);
                    Canvas.SetLeft(Arts_index, 40);
                    canvas.Children.Add(Arts_index);

                    if ((card["craft"] != null))
                    {
                        TextBlock Arts_craft = new TextBlock
                        {
                            Text = "크래프트",
                            Background = Brushes.Black,
                            Foreground = Brushes.White,
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center,
                            Width = 82,
                            Height = 24,
                            FontSize = 18,
                            FontWeight = FontWeights.Bold
                        };
                        Canvas.SetTop(Arts_craft, 36);
                        Canvas.SetLeft(Arts_craft, 144);
                        canvas.Children.Add(Arts_craft);
                    }

                        TextBlock Arts_name = new TextBlock
                    {
                        Text = name,
                        Background = Colorfeed(cardcolor),
                        Foreground = Brushes.Black,
                        TextWrapping = TextWrapping.NoWrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 728,
                        Height = 68,
                        FontSize = (card["namesize"] != null) ? card["namesize"].ToObject<int>() : 50,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetTop(Arts_name, 68);
                    Canvas.SetLeft(Arts_name, 136);
                    canvas.Children.Add(Arts_name);

                    for (int i = 0; i < timings.Count; i++)
                    {
                        TextBlock Arts_timing = new TextBlock
                        {
                            Text = timings[i].ToObject<String>(),
                            Background = Brushes.Black,
                            Foreground = Brushes.White,
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Width = 136,
                            Height = 32,
                            FontSize = 22,
                            FontWeight = FontWeights.Bold,
                        };
                        Canvas.SetBottom(Arts_timing, 436 + 44 * i);
                        Canvas.SetRight(Arts_timing, 68);
                        canvas.Children.Add(Arts_timing);
                    }

                    if ((card["limitation"] != null))
                    {
                        String limitation = card["limitation"].ToObject<String>();
                        TextBlock Arts_limitation = new TextBlock
                        {
                            Text = limitation,
                            Background = Brushes.Black,
                            Foreground = Brushes.White,
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center,
                            Width = limitationboxsize(limitation),
                            Height = 32,
                            FontSize = 22,
                            FontWeight = FontWeights.Bold,
                        };
                        Canvas.SetTop(Arts_limitation, 928);
                        Canvas.SetLeft(Arts_limitation, 68);
                        canvas.Children.Add(Arts_limitation);
                    }

                    RichTextBox Arts_text = new RichTextBox
                    {
                        Background = Colorfeed(cardcolor),
                        Foreground = Brushes.Black,
                        VerticalContentAlignment = VerticalAlignment.Top,
                        Width = 816,
                        Height = 270,
                        FontSize = (card["textsize"] != null) ? card["textsize"].ToObject<int>() : 27,
                        Padding = new Thickness(0, 5, 0, 5),
                        FontWeight = FontWeights.DemiBold,
                        IsReadOnly = true,
                        BorderThickness = new Thickness(0)
                    };

                    Paragraph paragraph = new Paragraph();

                    foreach (JArray line in card["effect"] as JArray)
                    {
                        foreach (var wordtoken in line)
                        {
                            String word = wordtoken.ToObject<String>();
                            wordadd(word, paragraph, (int)Arts_text.FontSize);
                        }
                        if (line != card["effect"].Last) paragraph.Inlines.Add("\n");
                    }
                    Arts_text.Document.Blocks.Clear();
                    Arts_text.Document.Blocks.Add(paragraph);


                    Canvas.SetTop(Arts_text, 1000);
                    Canvas.SetLeft(Arts_text, 92);
                    canvas.Children.Add(Arts_text);
                }
                if (category == "키")
                {
                    TextBlock Key_name = new TextBlock
                    {
                        Text = name,
                        Background = Colorfeed(cardcolor),
                        Foreground = Brushes.Black,
                        TextWrapping = TextWrapping.NoWrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 768,
                        Height = 68,
                        FontSize = (card["namesize"] != null) ? card["namesize"].ToObject<int>() : 50,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetTop(Key_name, 48);
                    Canvas.SetLeft(Key_name, 100);
                    canvas.Children.Add(Key_name);

                    TextBlock Key_index = new TextBlock
                    {
                        Text = "키",
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 68,
                        Height = 24,
                        FontSize = 18,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetTop(Key_index, 36);
                    Canvas.SetLeft(Key_index, 40);
                    canvas.Children.Add(Key_index);

                    if ((card["limitation"] != null))
                    {
                        String limitation = card["limitation"].ToObject<String>();
                        TextBlock Key_limitation = new TextBlock
                        {
                            Text = limitation,
                            Background = Brushes.Black,
                            Foreground = Brushes.White,
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center,
                            Width = limitationboxsize(limitation),
                            Height = 32,
                            FontSize = 22,
                            FontWeight = FontWeights.Bold,
                        };
                        Canvas.SetTop(Key_limitation, 644);
                        Canvas.SetLeft(Key_limitation, 68);
                        canvas.Children.Add(Key_limitation);
                    }

                    RichTextBox Key_text = new RichTextBox
                    {
                        Background = Colorfeed(cardcolor),
                        Foreground = Brushes.Black,
                        VerticalContentAlignment = VerticalAlignment.Top,
                        Width = 1184,
                        FontSize = (card["textsize"] != null) ? card["textsize"].ToObject<int>() : 27,
                        Padding = new Thickness(0, 5, 0, 5),
                        FontWeight = FontWeights.DemiBold,
                        IsReadOnly = true,
                        BorderThickness = new Thickness(0)
                    };

                    Paragraph paragraph = new Paragraph();

                    foreach (JArray line in card["effect"] as JArray)
                    {
                        foreach (var wordtoken in line)
                        {
                            String word = wordtoken.ToObject<String>();
                            wordadd(word, paragraph, (int)Key_text.FontSize);
                        }
                        if (line != card["effect"].Last) paragraph.Inlines.Add("\n");
                    }


                    Key_text.Document.Blocks.Clear();
                    Key_text.Document.Blocks.Add(paragraph);


                    Canvas.SetBottom(Key_text, 128);
                    Canvas.SetLeft(Key_text, 112);
                    canvas.Children.Add(Key_text);
                }

                if (category == "시그니")
                {
                    String signiclass = card["class"].ToObject<String>();

                    TextBlock Signi_index = new TextBlock
                    {
                        Text = "시그니",
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 76,
                        Height = 24,
                        FontSize = 18,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetTop(Signi_index, 36);
                    Canvas.SetLeft(Signi_index, 40);
                    canvas.Children.Add(Signi_index);

                    TextBlock Signi_name = new TextBlock
                    {
                        Text = name,
                        Background = Colorfeed(cardcolor),
                        Foreground = Brushes.Black,
                        TextWrapping = TextWrapping.NoWrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 728,
                        Height = 68,
                        FontSize = (card["namesize"] != null) ? card["namesize"].ToObject<int>() : 50,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetTop(Signi_name, 68);
                    Canvas.SetLeft(Signi_name, 136);
                    canvas.Children.Add(Signi_name);

                    TextBlock Signi_class = new TextBlock
                    {
                        Text = signiclass,
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Width = classboxsize(signiclass),
                        Height = 32,
                        FontSize = 22,
                        FontWeight = FontWeights.Bold,
                    };
                    Canvas.SetTop(Signi_class, 928);
                    Canvas.SetRight(Signi_class, 68);
                    canvas.Children.Add(Signi_class);

                    if ((card["limitation"] != null))
                    {
                        String limitation = card["limitation"].ToObject<String>();
                        TextBlock Signi_limitation = new TextBlock
                        {
                            Text = limitation,
                            Background = Brushes.Black,
                            Foreground = Brushes.White,
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center,
                            Width = limitationboxsize(limitation),
                            Height = 32,
                            FontSize = 22,
                            FontWeight = FontWeights.Bold,
                        };
                        Canvas.SetTop(Signi_limitation, 928);
                        Canvas.SetLeft(Signi_limitation, 68);
                        canvas.Children.Add(Signi_limitation);
                    }
                    if ((card["effect"] != null))
                    {
                        RichTextBox Signi_text = new RichTextBox
                        {
                            Background = Colorfeed(cardcolor),
                            Foreground = Brushes.Black,
                            VerticalContentAlignment = VerticalAlignment.Top,
                            Width = 816,
                            Height = 244,
                            FontSize = (card["textsize"] != null) ? card["textsize"].ToObject<int>() : 27,
                            Padding = new Thickness(0, 5, 0, 5),
                            FontWeight = FontWeights.DemiBold,
                            IsReadOnly = true,
                            BorderThickness = new Thickness(0)
                        };

                        Paragraph paragraph = new Paragraph();

                        foreach (JArray line in card["effect"] as JArray)
                        {
                            foreach (var wordtoken in line)
                            {
                                String word = wordtoken.ToObject<String>();
                                wordadd(word, paragraph, (int)Signi_text.FontSize);
                            }
                            if (line != card["effect"].Last) paragraph.Inlines.Add("\n");
                        }
                        Signi_text.Document.Blocks.Clear();
                        Signi_text.Document.Blocks.Add(paragraph);


                        Canvas.SetTop(Signi_text, 1000);
                        Canvas.SetLeft(Signi_text, 92);
                        canvas.Children.Add(Signi_text);
                    }

                    if ((card["lifeburst"] != null))
                    {
                        RichTextBox Signi_lifeburst = new RichTextBox
                        {
                            Background = Brushes.Black,
                            Foreground = Brushes.White,
                            VerticalContentAlignment = VerticalAlignment.Bottom,
                            Width = 816,
                            FontSize = (card["textsize"] != null) ? card["textsize"].ToObject<int>() : 27,
                            Padding = new Thickness(0, 5, 0, 5),
                            FontWeight = FontWeights.DemiBold,
                            IsReadOnly = true,
                            BorderThickness = new Thickness(0)
                        };

                        Paragraph burstparagraph = new Paragraph();

                        foreach (JArray line in card["lifeburst"] as JArray)
                        {
                            foreach (var wordtoken in line)
                            {
                                String word = wordtoken.ToObject<String>();
                                wordadd(word, burstparagraph, (int)Signi_lifeburst.FontSize);
                            }
                            if (line != card["lifeburst"].Last) burstparagraph.Inlines.Add("\n");
                        }
                        Signi_lifeburst.Document.Blocks.Clear();
                        Signi_lifeburst.Document.Blocks.Add(burstparagraph);


                        Canvas.SetBottom(Signi_lifeburst, 150);
                        Canvas.SetLeft(Signi_lifeburst, 92);
                        canvas.Children.Add(Signi_lifeburst);
                    }

                }

                if (category == "스펠")
                {

                    TextBlock Spell_index = new TextBlock
                    {
                        Text = "스펠",
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 76,
                        Height = 24,
                        FontSize = 18,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetTop(Spell_index, 36);
                    Canvas.SetLeft(Spell_index, 40);
                    canvas.Children.Add(Spell_index);

                    TextBlock Spell_name = new TextBlock
                    {
                        Text = name,
                        Background = Colorfeed(cardcolor),
                        Foreground = Brushes.Black,
                        TextWrapping = TextWrapping.NoWrap,
                        TextAlignment = TextAlignment.Center,
                        Width = 728,
                        Height = 68,
                        FontSize = (card["namesize"] != null) ? card["namesize"].ToObject<int>() : 50,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetTop(Spell_name, 68);
                    Canvas.SetLeft(Spell_name, 136);
                    canvas.Children.Add(Spell_name);

                    if ((card["limitation"] != null))
                    {
                        String limitation = card["limitation"].ToObject<String>();
                        TextBlock Spell_limitation = new TextBlock
                        {
                            Text = limitation,
                            Background = Brushes.Black,
                            Foreground = Brushes.White,
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center,
                            Width = limitationboxsize(limitation),
                            Height = 32,
                            FontSize = 22,
                            FontWeight = FontWeights.Bold,
                        };
                        Canvas.SetTop(Spell_limitation, 928);
                        Canvas.SetLeft(Spell_limitation, 68);
                        canvas.Children.Add(Spell_limitation);
                    }

                    RichTextBox Spell_text = new RichTextBox
                    {
                        Background = Colorfeed(cardcolor),
                        Foreground = Brushes.Black,
                        VerticalContentAlignment = VerticalAlignment.Top,
                        Width = 816,
                        Height = 244,
                        FontSize = (card["textsize"] != null) ? card["textsize"].ToObject<int>() : 27,
                        Padding = new Thickness(0, 5, 0, 5),
                        FontWeight = FontWeights.DemiBold,
                        IsReadOnly = true,
                        BorderThickness = new Thickness(0)
                    };

                    Paragraph paragraph = new Paragraph();

                    foreach (JArray line in card["effect"] as JArray)
                    {
                        foreach (var wordtoken in line)
                        {
                            String word = wordtoken.ToObject<String>();
                            wordadd(word, paragraph, (int)Spell_text.FontSize);
                        }
                        if (line != card["effect"].Last) paragraph.Inlines.Add("\n");
                    }
                    Spell_text.Document.Blocks.Clear();
                    Spell_text.Document.Blocks.Add(paragraph);


                    Canvas.SetTop(Spell_text, 1000);
                    Canvas.SetLeft(Spell_text, 92);
                    canvas.Children.Add(Spell_text);

                    if ((card["lifeburst"] != null))
                    {
                        RichTextBox Spell_lifeburst = new RichTextBox
                        {
                            Background = Brushes.Black,
                            Foreground = Brushes.White,
                            VerticalContentAlignment = VerticalAlignment.Bottom,
                            Width = 816,
                            FontSize = (card["textsize"] != null) ? card["textsize"].ToObject<int>() : 27,
                            Padding = new Thickness(0, 5, 0, 7),
                            FontWeight = FontWeights.DemiBold,
                            IsReadOnly = true,
                            BorderThickness = new Thickness(0)
                        };

                        Paragraph burstparagraph = new Paragraph();

                        foreach (JArray line in card["lifeburst"] as JArray)
                        {
                            foreach (var wordtoken in line)
                            {
                                String word = wordtoken.ToObject<String>();
                                wordadd(word, burstparagraph, (int)Spell_lifeburst.FontSize);
                            }
                            if (line != card["lifeburst"].Last) paragraph.Inlines.Add("\n");
                        }
                        Spell_lifeburst.Document.Blocks.Clear();
                        Spell_lifeburst.Document.Blocks.Add(burstparagraph);


                        Canvas.SetBottom(Spell_lifeburst, 150);
                        Canvas.SetLeft(Spell_lifeburst, 92);
                        canvas.Children.Add(Spell_lifeburst);
                    }

                }






                this.Content = canvas;
                canvas.Measure(new Size(width, height));
                canvas.Arrange(new Rect(0, 0, width, height));

                Rect rect = new Rect(0, 0, canvas.Width, canvas.Height);
                double dpi = 96d;
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right, (int)rect.Bottom, dpi, dpi, System.Windows.Media.PixelFormats.Default);
                rtb.Render(canvas);

                BitmapEncoder pngEncoder = new PngBitmapEncoder();

                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData(imgurl);
                MemoryStream mss = new MemoryStream(bytes);
                System.Drawing.Image imgg = System.Drawing.Image.FromStream(mss);


                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    dc.DrawImage(ToWpfBitmapImage(imgg), rect);
                    dc.DrawImage(rtb, rect);
                }
                RenderTargetBitmap res = new RenderTargetBitmap((int)rect.Right, (int)rect.Bottom, dpi, dpi, PixelFormats.Pbgra32);
                res.Render(dv);

                pngEncoder.Frames.Add(BitmapFrame.Create(res));

                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();

                    pngEncoder.Save(ms);
                    ms.Close();

                    System.IO.File.WriteAllBytes(@"..\..\Processed\" + packindex + @"\" + card["no"].ToObject<String>() + ".png", ms.ToArray());
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }


        }
    }
}
