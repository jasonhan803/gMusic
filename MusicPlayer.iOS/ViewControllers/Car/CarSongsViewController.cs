﻿using System;
using MusicPlayer.iOS.ViewControllers;
using UIKit;

namespace MusicPlayer.iOS.Car
{
	class CarSongsViewController : SongViewController
	{
		public CarSongsViewController()
		{
			
		}

		UIGestureRecognizer panGesture;
		public override void LoadView()
		{
			base.LoadView();
			TableView.SectionIndexMinimumDisplayRowCount = 2000000;
			//panGesture = TableView.AddGestures();
		}

		CoreGraphics.CGSize lastSize;
		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();
			var size = View.Bounds.Size;
			if (lastSize == size)
				return;
			lastSize = size;
			if (menuButton != null) {
				menuButton.Image = Images.GetMenuImage (size.Width * CarStyle.NavIconImagePercent);
			}
			TableView.RowHeight = CarStyle.RowHeight;
			TableView.ReloadData();
		}

	}
}