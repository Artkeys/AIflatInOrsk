using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace AIProperty
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

            //Заполнение ComboBox значениями
			string[] district = new string[2] {"Отдаленный район", "Центр"};
			string[] TehSost = new string[3] { "Требует ремонта", "Ремонт", "Хорошее" };
			string[] TypeBuilding = new string[3] { "Шлакоблочный", "Панельный", "Кирпичный" };

			DistrictCmb.ItemsSource = district;
			TehSostCmb.ItemsSource = TehSost;
			TypeBuildingCmb.ItemsSource = TypeBuilding;
		}

        //Глобальные переменные
        static int neirons = 100;
        double[,] W1 = new double[7, neirons];
        double[,] W2 = new double[7, neirons];
        double[] W3 = new double[neirons];

        //Функция обучения нейронной сети
        private void Teaching()
        {
            int[,] x = new int[30, 7]{
                                 {2,44, 1,  1,  5,  2,  1},
                                 {3,45, 4,  1,  5,  1,  1},
                                 {2,45, 4,  1,  5,  2,  1},
                                 {2 ,45,4,  0,  5,  1,  1},
                                 {2 ,45,1,  0,  5,  1,  0},
                                 {2 ,61,3,  1,  3,  0,  0},
                                 {3 ,62,8,  1,  9,  2,  2},
                                 {2 ,50,2,  1,  4,  2,  0},
                                 {2 ,53,1,  0,  9,  2,  1},
                                 {1 ,30,5,  0,  5,  1,  1},
                                 {1 ,32,1,  0,  9,  1,  2},
                                 {2 ,44,4,  0,  5,  0,  1},
                                 {2 ,47,1,  1,  5,  2,  0},
                                 {2 ,46,1,  0,  5,  2,  0},
                                 {3 ,59,2,  1,  3,  2,  2},
                                 {1 ,32,2,  1,  2,  2,  2},
                                 {3 ,63,6,  0,  9,  2,  0},
                                 {3 ,72,3,  1,  3,  2,  1},
                                 {1 ,33,1,  0,  9,  2,  1},
                                 {2 ,45,4,  1,  5,  2,  1},
                                 {3 ,62,4,  0,  5,  2,  0},
                                 {2 ,45,1,  0,  2,  2,  1},
                                 {3 ,61,2,  1,  2,  2,  0},
                                 {3 ,80,2,  1,  3,  2,  1},
                                 {3 ,65,3,  1,  9,  2,  0},
                                 {3 ,93,2,  1,  4,  2,  0},
                                 {2 ,45,1,  1,  5,  2,  2},
                                 {3 ,63,2,  1,  2,  2,  0},
                                 {2 ,53,2,  1,  5,  2,  0},
                                 {1 ,46,4,  0,  5,  0,  1}};
            double[] yReal = new double[] { 0.113, 0.125, 0.104, 0.065, 0.1, 0.12, 0.19, 0.12, 0.105, 0.065, 0.069, 0.085, 0.115, 0.11, 0.145, 0.063, 0.177, 0.238, 0.085, 0.105, 0.115, 0.095, 0.125, 0.185, 0.223, 0.247, 0.082, 0.147, 0.1, 0.085 };
            double Sum = 0;
            double[] y1 = new double[neirons];
            double[] y2 = new double[neirons];
            double y3 = 0;
            double[] delta1 = new double[neirons];
            double[] delta2 = new double[neirons];
            double Epsilon = 0.01;
            double SumError = 0;
            double h = 0.1;
            double delta3 = 0;
            double E;
            int Prohod = 0;

            Random Rnd = new Random();

            for (int j = 0; j < neirons; j++)
            {
                y1[j] = 0;
                y2[j] = 0;
                delta1[j] = 0;
                delta2[j] = 0;
            }
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < neirons; j++)
                {
                    W1[i, j] = Rnd.NextDouble() * (1 - (-1)) + (-1);
                    W2[i, j] = Rnd.NextDouble() * (1 - (-1)) + (-1);
                }
            }

            for (int j = 0; j < neirons; j++)
            {
                W3[j] = Rnd.NextDouble() * (1 - (-1)) + (-1);
            }

            while (true)
            {
                SumError = 0;
                for (int n = 0; n < 30; n++)
                {
                    for (int j = 0; j < neirons; j++)
                    {
                        Sum = 0;
                        for (int i = 0; i < 7; i++)
                        {
                            Sum += W1[i, j] * x[n, i];
                        }
                        y1[j] = 1 / (1 + (Math.Exp(-Sum)));
                    }

                    for (int j = 0; j < neirons; j++)
                    {
                        Sum = 0;
                        for (int i = 0; i < 7; i++)
                        {
                            Sum += W2[i, j] * y1[j];
                        }
                        y2[j] = 1 / (1 + (Math.Exp(-Sum)));
                    }

                    Sum = 0;
                    for (int j = 0; j < neirons; j++)
                    {
                        Sum += W3[j] * y2[j];
                    }
                    y3 = 1 / (1 + (Math.Exp(-Sum)));

                    SumError += Math.Pow((yReal[n] - y3), 2);

                    delta3 = (yReal[n] - y3) * y3 * (1 - y3);

                    for (int j = 0; j < neirons; j++)
                    {
                        W3[j] += h * delta3 * y2[j];
                    }

                    for (int j = 0; j < neirons; j++)
                    {
                        delta2[j] = delta3 * W3[j] * y2[j] * (1 - y2[j]);
                    }

                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < neirons; j++)
                        {
                            W2[i, j] += h * delta2[j] * y1[j];
                        }
                    }
                    for (int j = 0; j < neirons; j++)
                    {
                        Sum = 0;
                        for (int i = 0; i < 7; i++)
                        {
                            Sum += delta2[j] * W2[i, j] * y1[j] * (1 - y1[j]);
                        }
                        delta1[j] = Sum;
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < neirons; j++)
                        {
                            W1[i, j] += h * delta1[j] * x[n, i];
                        }
                    }
                }
                E = 0.5 * SumError;
                //epoh_NetworkList.Items.Add(E);
                this.Dispatcher.Invoke(() => { epoh_NetworkList.Items.Add("Ошибка = " + E); epoh_NetworkList.ScrollIntoView(epoh_NetworkList.Items[epoh_NetworkList.Items.Count - 1]); if (Prohod % 500 == 0) epoh_NetworkList.Items.Clear(); });
                Prohod++;
                if (E <= Epsilon)
                {
                    MessageBox.Show("Сеть обучена", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Dispatcher.Invoke(() => { epoh_NetworkList.Items.Add($"Сеть обучилась за {Prohod} проходов"); CalculateBtn.IsEnabled = true; epoh_NetworkList.ScrollIntoView(epoh_NetworkList.Items[epoh_NetworkList.Items.Count - 1]); });
                    break;
                }
            }
        }

        //Кнопка для осуществления обучения
		private void teach_networkBtn_Click(object sender, RoutedEventArgs e)
		{
            epoh_NetworkList.Focus();
            var thread = new Thread(Teaching);
            thread.Start();
        }

        //Функция расчета стоимости недвижимости
        void Calculate_Price()
		{
            double[] Vhod = new double[7] {Convert.ToInt32(NumberRoomsTxt.Text), Convert.ToInt32(TotalAreaTxt.Text), Convert.ToInt32(FloorAreaTxt.Text), DistrictCmb.SelectedIndex, Convert.ToInt32(FloorBuildingTxt.Text), TehSostCmb.SelectedIndex, TypeBuildingCmb.SelectedIndex};
            double Sum = 0;
            double[] y1 = new double[neirons];
            double[] y2 = new double[neirons];
            double y3 = 0;
            for (int n = 0; n < 30; n++)
            {
                for (int j = 0; j < neirons; j++)
                {
                    Sum = 0;
                    for (int i = 0; i < 7; i++)
                    {
                        Sum += W1[i, j] * Vhod[i];
                    }
                    y1[j] = 1 / (1 + (Math.Exp(-Sum)));
                    //Console.WriteLine(y1[j]);
                }

                for (int j = 0; j < neirons; j++)
                {
                    Sum = 0;
                    for (int i = 0; i < 7; i++)
                    {
                        Sum += W2[i, j] * y1[j];
                    }
                    y2[j] = 1 / (1 + (Math.Exp(-Sum)));
                }

                Sum = 0;
                for (int j = 0; j < neirons; j++)
                {
                    Sum += W3[j] * y2[j];
                }
                y3 = 1 / (1 + (Math.Exp(-Sum)));
            }
            MessageBox.Show($"Примерная цена недвижимости - {Convert.ToInt32(y3 * 10000000)} рублей", "Стоимость недвижимости", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Нажатие на кнопку рассчитать
		private void CalculateBtn_Click(object sender, RoutedEventArgs e)
		{
            //Проверка на ввод значений в поля

            if (NumberRoomsTxt.Text == "")
            {
                MessageBox.Show("Введите количество комнат!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                NumberRoomsTxt.Focus();
                return;
            }

            if (TotalAreaTxt.Text == "")
            {
                MessageBox.Show("Введите площадь недвижимости!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                TotalAreaTxt.Focus();
                return;
            }

            if (FloorAreaTxt.Text == "")
            {
                MessageBox.Show("Введите этаж недвижимости!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                FloorAreaTxt.Focus();
                return;
            }

            if (DistrictCmb.Text == "")
            {
                MessageBox.Show("Выберите район недвижимости!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                DistrictCmb.Focus();
                return;
            }

            if (FloorBuildingTxt.Text == "")
            {
                MessageBox.Show("Введите этажность недвижимости!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                FloorBuildingTxt.Focus();
                return;
            }

            if (TehSostCmb.Text == "")
            {
                MessageBox.Show("Выберите техническое состояние недвижимости!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                TehSostCmb.Focus();
                return;
            }

            if (TypeBuildingCmb.Text == "")
            {
                MessageBox.Show("Выберите тип недвижимости!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                TypeBuildingCmb.Focus();
                return;
            }
            
            Calculate_Price();
		}

        //Защита полей от неккоректного ввода
		private void NumberRoomsTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

		private void TotalAreaTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

		private void FloorAreaTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

		private void FloorBuildingTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

		private void ClearBtn_Click(object sender, RoutedEventArgs e)
		{
            NumberRoomsTxt.Text = "";
            TotalAreaTxt.Text = "";
            FloorAreaTxt.Text = "";
            DistrictCmb.Text = "";
            FloorBuildingTxt.Text = "";
            TehSostCmb.Text = "";
            TypeBuildingCmb.Text = "";
        }
	}
}
