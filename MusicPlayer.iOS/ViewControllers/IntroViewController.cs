﻿using System;
using MusicPlayer.Managers;
using UIKit;
using CoreGraphics;
using System.Threading.Tasks;
using Localizations;
using MusicPlayer.Data;

namespace MusicPlayer.iOS
{
	public class IntroViewController : UIViewController
	{
		public IntroViewController()
		{
			Title = "Welcome";
		}
		public async Task Login()
		{
			try
			{
				var vc = new ServicePickerViewController ();
				this.PresentModalViewController (new UINavigationController (vc), true);
				var service = await vc.GetServiceTypeAsync ();
				var s = await ApiManager.Shared.CreateAndLogin (service);
				if (s) {
					await this.DismissViewControllerAsync (true);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		public async Task Skip ()
		{
			Settings.IncludeIpod = Settings.IPodOnly = true;
			await this.DismissViewControllerAsync (true);
		}

		IntroView view;
		public override void LoadView()
		{
			View = view = new IntroView();
		}
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			view.login.Tapped = async (b)=> await Login();
			view.signinLater.Tapped = async (b) => await Skip ();
        }
		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			view.login.Tapped = null;
			view.signinLater.Tapped = null;
		}

		class IntroView : UIView
		{
			UITextView textView;
			UITextView headerText;
			UIImageView image;
			public SimpleButton login;
			public SimpleButton signinLater;
			UIView blurView;
            public IntroView()
			{
				BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("launchBg"));
				Add(blurView = new BluredView(UIBlurEffectStyle.Light));
				Add(image = new UIImageView(UIImage.FromBundle("headphones")));
				if (!Device.IsIos8)
				{
					blurView.Alpha = 0;
					image.Alpha = 0;
				}

				Add(headerText = new UITextView
				{
					Text = Strings.WelcomToGmusic,
					TextAlignment = UITextAlignment.Center,
					TextColor = UIColor.White,
					Font = Style.DefaultStyle.HeaderTextThinFont,
					BackgroundColor = UIColor.Clear,
				});
				//Add(textView = new UITextView {
				//	Text = Text,
				//	TextAlignment = UITextAlignment.Center,
				//	TextColor = UIColor.White,
				//	BackgroundColor = UIColor.Clear,
				//});

				Add(login = new SimpleButton {
					Text = Strings.Login,
				}.StyleAsBorderedButton());
				login.SizeToFit(); 

				Add (signinLater = new SimpleButton {
					Text = Strings.Skip,
				}.StyleAsTextButton ());
				signinLater.SizeToFit ();
			}
		
			public override void LayoutSubviews()
			{
				base.LayoutSubviews();
				var bounds = Bounds;
				var x = bounds.Width/4;
				var width = x*2;
				var y = bounds.Height / 4;
				blurView.Frame = bounds;

				var imageWidth =  Math.Min(image.Image.Size.Width,width *.6);
				var imageX = (bounds.Width - imageWidth)/2;
				image.Frame = new CGRect(imageX,y,imageWidth,imageWidth);

				y = image.Frame.Bottom + 10f;

				var size = headerText.SizeThatFits(new CGSize(width,1000));

				headerText.Frame = new CGRect(x,y,width,size.Height);

				y = headerText.Frame.Bottom + 10;

				var frame = signinLater.Frame;
				frame.Width = width;
				frame.X = x;
				frame.Y = bounds.Bottom - frame.Height - 30f;
				signinLater.Frame = frame;

				frame.Height = login.Frame.Height;
				frame.Y -= frame.Height + 10f;
				login.Frame = frame;




				//textView.Frame = new CGRect(x,y,width,frame.Y - y);


			}
		}
	}
}