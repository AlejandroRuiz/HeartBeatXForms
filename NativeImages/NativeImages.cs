using System;

using Xamarin.Forms;
using NativeImages.Controls;
using System.Threading.Tasks;

namespace NativeImages
{
	public class App : Application
	{
		HeartBeatView heartBeatView;
		public App ()
		{
			heartBeatView = new HeartBeatView {
				Source = "Heart",
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};  
			// The root page of your application
			MainPage = new ContentPage {
				Content = new StackLayout {
					Children = {
						heartBeatView
					}
				}
			};
		}

		protected async override void OnStart ()
		{
			// Handle when your app starts
			heartBeatView.IsBeating = true;
			await Task.Delay(5000);
			heartBeatView.IsBeating = false;
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

