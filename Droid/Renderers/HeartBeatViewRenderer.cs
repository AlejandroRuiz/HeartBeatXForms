using System;
using Android.Widget;
using NativeImages.Controls;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NativeImages.Droid.Renderers;
using System.Threading.Tasks;
using NativeImages.Extentions;
using Android.Views.Animations;

[assembly: ExportRenderer (typeof(HeartBeatView), typeof(HeartBeatViewRenderer))]
namespace NativeImages.Droid.Renderers
{
	public class HeartBeatViewRenderer:ViewRenderer<HeartBeatView, ImageView>
	{
		public HeartBeatViewRenderer ()
		{
		}

		protected async override void OnElementChanged (ElementChangedEventArgs<HeartBeatView> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement == null) {
				SetNativeControl (new ImageView(this.Context));
			}

			if (e.NewElement != null) {
				await BuildHeartBeat ();
				UpdateBeatState ();
			}
		}

		protected async override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == HeartBeatView.SourceProperty.PropertyName) {
				await BuildHeartBeat ();
			}else if (e.PropertyName == HeartBeatView.IsBeatingProperty.PropertyName) {
				UpdateBeatState ();
			}
		}

		async Task BuildHeartBeat()
		{
			var imageHandler = base.Element.Source.GetHandler ();
			if (imageHandler != null) {
				var nativeImage = await imageHandler.LoadImageAsync (base.Element.Source, this.Context);
				if (nativeImage != null) {
					Device.BeginInvokeOnMainThread (() => {
						this.Control.SetImageBitmap (nativeImage);
					});
				}
			}
		}

		void UpdateBeatState()
		{
			if (base.Element.IsBeating) {
				Action act = new Action (() => {
					ScaleAnimation Ani = new ScaleAnimation (1, 1.2f, 1, 1.2f, base.Control.Width / 2, base.Control.Height / 2);
					Ani.RepeatCount = ScaleAnimation.Infinite;
					Ani.Duration = 750;
					Ani.Interpolator = new HeartBeatInterpolator ();
					base.Control.StartAnimation (Ani);
				});
				base.Control.Post (act);
			} else {
				base.Control.ClearAnimation ();
			}
		}

		class HeartBeatInterpolator:Java.Lang.Object, IInterpolator
		{
			public float GetInterpolation (float input)
			{
				float x = input < 1/3f? 2 * input : (1 + input) / 2;
				return (float) Math.Sin(x * Math.PI);
			}
		}
	}
}

