using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace _0lymp.us.Gods
{
	public partial class Iris : Component
	{

		public delegate void CommunicateCompleteHandler(JObject Response);
		public delegate void CommunicateCompleteHandler2(JObject Response);
		public delegate void CommunicateCompleteHandler3(JObject Response);
		public delegate void CommunicateCompleteHandler4(JObject Response);

		public NameValueCollection data;
		public string url;
		public NameValueCollection data2;
		public string url2;
		public NameValueCollection data3;
		public string url3;
		public NameValueCollection data4;
		public string url4;

		public Thread CommunicateThread;
		public Thread CommunicateThread2;
		public Thread CommunicateThread3;
		public Thread CommunicateThread4;

		public event CommunicateCompleteHandler CommunicateComplete;
		public event CommunicateCompleteHandler2 CommunicateComplete2;
		public event CommunicateCompleteHandler3 CommunicateComplete3;
		public event CommunicateCompleteHandler4 CommunicateComplete4;

		public Iris()
		{
			InitializeComponent();
		}

		public Iris(IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}

		public void Communicate()
		{
			try
			{
				WebClient webClient = new WebClient();
				byte[] bytes = webClient.UploadValues(url, "POST", data);
				string @string = Encoding.UTF8.GetString(bytes);
				JObject response = JObject.Parse(@string);
				this.CommunicateComplete(response);
			}
			catch (WebException)
			{
				KillThread();
				ExecuteThread();
			}
			catch (NullReferenceException)
			{
			}
		}

		public void ExecuteThread()
		{
			CommunicateThread = new Thread(Communicate);
			CommunicateThread.Start();
		}

		public void KillThread()
		{
			CommunicateThread.Abort();
			CommunicateThread.Join();
		}

		public void Communicate2()
		{
			try
			{
				WebClient webClient = new WebClient();
				byte[] bytes = webClient.UploadValues(url2, "POST", data2);
				string @string = Encoding.UTF8.GetString(bytes);
				JObject response = JObject.Parse(@string);
				this.CommunicateComplete2(response);
			}
			catch (WebException)
			{
				KillThread2();
				ExecuteThread2();
			}
			catch (NullReferenceException)
			{
			}
		}

		public void ExecuteThread2()
		{
			CommunicateThread2 = new Thread(Communicate2);
			CommunicateThread2.Start();
		}

		public void KillThread2()
		{
			CommunicateThread2.Abort();
			CommunicateThread2.Join();
		}

		public void Communicate3()
		{
			try
			{
				WebClient webClient = new WebClient();
				byte[] bytes = webClient.UploadValues(url3, "POST", data3);
				string @string = Encoding.UTF8.GetString(bytes);
				JObject response = JObject.Parse(@string);
				this.CommunicateComplete3(response);
			}
			catch (WebException)
			{
				KillThread3();
				ExecuteThread3();
			}
			catch (NullReferenceException)
			{
			}
		}

		public void ExecuteThread3()
		{
			CommunicateThread3 = new Thread(Communicate3);
			CommunicateThread3.Start();
		}

		public void KillThread3()
		{
			CommunicateThread3.Abort();
			CommunicateThread3.Join();
		}

		public void Communicate4()
		{
			try
			{
				WebClient webClient = new WebClient();
				byte[] bytes = webClient.UploadValues(url4, "POST", data4);
				string @string = Encoding.UTF8.GetString(bytes);
				JObject response = JObject.Parse(@string);
				this.CommunicateComplete4(response);
			}
			catch (WebException)
			{
				KillThread4();
				ExecuteThread4();
			}
			catch (NullReferenceException)
			{
			}
			catch (JsonReaderException)
			{
				MessageBox.Show("Debe digitar al menos los primeros 6 digitos del BIN", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		public void ExecuteThread4()
		{
			CommunicateThread4 = new Thread(Communicate4);
			CommunicateThread4.Start();
		}

		public void KillThread4()
		{
			CommunicateThread4.Abort();
			CommunicateThread4.Join();
		}
	}
}
