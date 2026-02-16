

import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';

abstract class GeneralUtil {
  /// Show a toast to the user
  static void showToast(String message) {
    Fluttertoast.showToast(msg: message, toastLength: Toast.LENGTH_LONG);
  }

  static void goToPage(BuildContext context, Widget page, {bool? keepRoute}) {
    if (keepRoute ?? true) {
      Navigator.push(context, MaterialPageRoute(builder: (context) => page));
    } else {
      Navigator.pushAndRemoveUntil(context, MaterialPageRoute(builder: (context) => page), (route) => false);
    }
  }
}