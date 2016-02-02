using System;
using Xamarin.Forms;

namespace NativeImages.Controls
{
	public class HeartBeatView:View
	{
		public HeartBeatView ()
		{
		}

		public static readonly BindableProperty SourceProperty =
			BindableProperty.Create<HeartBeatView, ImageSource> (
				p => p.Source, null);

		public ImageSource Source
		{
			get {
				return (ImageSource)GetValue(SourceProperty);
			}
			set {
				SetValue(SourceProperty, value);
			}
		}

		public static readonly BindableProperty IsBeatingProperty =
			BindableProperty.Create<HeartBeatView, bool> (
				p => p.IsBeating, false);

		public bool IsBeating
		{
			get {
				return (bool)GetValue(IsBeatingProperty);
			}
			set {
				SetValue(IsBeatingProperty, value);
			}
		}
	}
}

