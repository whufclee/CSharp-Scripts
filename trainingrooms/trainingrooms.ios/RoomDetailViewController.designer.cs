// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace trainingrooms.ios
{
    [Register ("RoomDetailViewController")]
    partial class RoomDetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel txtLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel txtRoomName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (txtLocation != null) {
                txtLocation.Dispose ();
                txtLocation = null;
            }

            if (txtRoomName != null) {
                txtRoomName.Dispose ();
                txtRoomName = null;
            }
        }
    }
}