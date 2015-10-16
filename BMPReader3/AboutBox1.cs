using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPview.Resources
{
	partial class AboutBox1 : Form
	{
		int sw = 0;
        #region Комментарий

        public String comment =
                        "v.3.12.97" + "\r\n" +
                        " - Изображение в окне с увеличением (по левой кнопке мыши) теперь автоматически обновляется." + "\r\n" +
                        "v.3.11.94" + "\r\n" +
                        " - Исправлен еще один косяк с вырезкой части файла." + "\r\n" +
                        "v.3.11.93" + "\r\n" +
						" - Добавлен костылик под картинку 640х512" + "\r\n" +
						"v.3.11.92" + "\r\n" +
						" - Исправлен косяк с вырезкой части файла. Странно... Раньше работало..." + "\r\n" +
						"v.3.11.91" + "\r\n" +
						" - Добавлена возможность отключения контрастирования (всех видов)." + "\r\n" +
						"v.3.11.90" + "\r\n" +
						" - При включенном РА в AVI сохраняются две картинки." + "\r\n" +
						"v.3.10.88" + "\r\n" +
						" - Добавлена фильтрация одиночных точек. Вроде работает." + "\r\n" +
						"v.3.10.86" + "\r\n" +
						" - Добавлены синхронные перевороты картинки РА." + "\r\n" +
						"v.3.10.84" + "\r\n" +
						" - Вроде как работает." + "\r\n" +
						"v.3.10.83" + "\r\n" +
						" - Первая попытка добавить разностный алгоритм." + "\r\n" +
						"v.3.9.82" + "\r\n" +
						" - При перевернутой картинке отбивает правильно. Не доделал еще поворот на 90." + "\r\n" +
						"v.3.9.81" + "\r\n" +
						" - Перестановка пикселов включена по умолчанию." + "\r\n" +
						" - Добавлен поиск вспышки (критерий - изменение яркости пикселя на определенное значение в соседних кадрах)." + "\r\n" +
						" - При перевернутой картинке отбивает неправильно. Не доделал еще." + "\r\n" +
						"v.3.8.75" + "\r\n" +
						" - Немного оптимизирован код. Вроде как 3 fps добавилось." + "\r\n" +
						"v.3.8.73" + "\r\n" +
						" - Устранен мелкий косяк с номерами кадров." + "\r\n" +
						" - Устранен косяк с изображением при загрузке файла." + "\r\n" +
						" - Добавлен альтернативный режим контрастирования изображения (иногда помогает)" + "\r\n" +
						"v.3.8.66" + "\r\n" +
						" - Устранен мелкий косяк с загрузкой файла и номерами кадров." + "\r\n" +
						" - Устранен мелкий косяк изображением при окончании проигрывания файла." + "\r\n" +
						"v.3.8.65" + "\r\n" +
                        " - Убрано окно с координатами и яркостью пикселя при клике" + "\r\n" +
                        " - Добавлено всплывающее окно с увеличением х5 при нажатии правой кнопки" + "\r\n" +
                        "v.3.7.61" + "\r\n" +
                        " - Перестановка пикселей теперь работает и при сохранении данных в файл CSV" + "\r\n" +
                        "v.3.7.60" + "\r\n" +
                        " - Доделано выделение фрагмента при переворотах изображения. Теперь мин. и макс. значения считаются правильно." + "\r\n" +
                        " - Теперь при клике на изображении выдаются координаты, номер и яркость пикселя." + "\r\n" +
                        "v.3.7.59" + "\r\n" +
                        " - Устранен мелкий косяк с записью видео" + "\r\n" +
                        " - Устранен еще один мелкий косяк с кликами по изображению." + "\r\n" +
                        "v.3.7.57" + "\r\n" +
                        " - Добавлено сохранение данных в файл CSV" + "\r\n" +
                        " - Добавлена возможность сохранения нескольких файлов AVI и CSV - создаются с уникальными именами " + "\r\n" +
                        "v.3.6.54" + "\r\n" +
                        " - Добавлено сохранение в видеофайл AVI. Если в системе установлен кодек, он используется, иначе видео пишется без сжатия" + "\r\n" +
                        "v.3.4.42" + "\r\n" +
                        " - Добавлена возможность выделения фрагмента изображения с подсчетом мин. и макс. значений. При перевороте изображения работает криво!!! Пока не доделано!!!" + "\r\n" +
                        "";
        #endregion
		public AboutBox1()
		{
			InitializeComponent();
			this.Text = String.Format("О программе {0}", AssemblyTitle);
			this.labelProductName.Text = AssemblyProduct;
			this.labelVersion.Text = String.Format("Версия {0}", AssemblyVersion);
			this.labelCopyright.Text = AssemblyCopyright;
			this.labelCompanyName.Text = AssemblyCompany;
			this.label1.Text = AssemblyDescription;
            this.textBoxDescription.Text = comment;
		}

		#region Методы доступа к атрибутам сборки

		public string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "")
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}
		#endregion

		private void AboutBox1_Load(object sender, EventArgs e)
		{

		}

		private void logoPictureBox_MouseEnter(object sender, EventArgs e)
		{
			timer1.Enabled = true;
		}

		private void logoPictureBox_MouseLeave(object sender, EventArgs e)
		{
			timer1.Enabled = false;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (sw == 0)
			{
				logoPictureBox.Image = imageList1.Images[sw];
				sw = 1;
			}
			else
			{
				logoPictureBox.Image = imageList1.Images[sw];
				sw = 0;
			}
		}
	}
}
