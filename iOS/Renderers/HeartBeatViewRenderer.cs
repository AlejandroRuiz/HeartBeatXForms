using System;
using Xamarin.Forms;
using NativeImages.Controls;
using NativeImages.iOS.Renderers;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using NativeImages.Extentions;
using CoreGraphics;
using CoreAnimation;
using Foundation;

[assembly: ExportRenderer (typeof(HeartBeatView), typeof(HeartBeatViewRenderer))]
namespace NativeImages.iOS.Renderers
{
	public class HeartBeatViewRenderer:ViewRenderer<HeartBeatView, UIImageView>
	{
		public HeartBeatViewRenderer ()
		{
		}

		protected async override void OnElementChanged (ElementChangedEventArgs<HeartBeatView> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement == null) {
				SetNativeControl (new UIImageView (this.Frame));
				base.Control.ContentMode = UIViewContentMode.ScaleAspectFit;
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
				var nativeImage = await imageHandler.LoadImageAsync (base.Element.Source);
				if (nativeImage != null) {
					Device.BeginInvokeOnMainThread (() => {
						this.Control.Image = nativeImage;
					});
				}
			}
		}

		void UpdateBeatState()
		{
			var hasAnimationRegistered = base.Control.Layer.AnimationForKey ("HeartAnimation") != null;
			if (base.Element.IsBeating) {
				if (!hasAnimationRegistered) {
					CABasicAnimation animation = CABasicAnimation.FromKeyPath ("transform.scale");
					animation.AutoReverses = true;
					animation.From = new NSNumber (1);
					animation.To = new NSNumber (1.2f);
					animation.Duration = 0.5;
					animation.RepeatCount = int.MaxValue;
					animation.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseIn);
					base.Control.Layer.AddAnimation (animation, "HeartAnimation");
				}
			} else {
				if(hasAnimationRegistered){
					base.Control.Layer.RemoveAnimation("HeartAnimation");
				}
			}
		}
	}
}

