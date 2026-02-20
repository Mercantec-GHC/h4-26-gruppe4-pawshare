import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';

abstract class GeneralUtil {
  /// Checks if a string is a valid base64 string
  static bool isBase64(String s) {
    try {
      base64.decode(s);
      return true;
    } catch (_) {
      return false;
    }
  }
  
  /// Show a toast to the user
  static void showToast(String message) {
    Fluttertoast.showToast(msg: message, toastLength: Toast.LENGTH_LONG);
  }

  /// Navigates to a page, optionally keeping the previous route
  static void goToPage(BuildContext context, Widget page, {bool? keepRoute}) {
    if (keepRoute ?? true) {
      Navigator.push(context, MaterialPageRoute(builder: (context) => page));
    } else {
      Navigator.pushAndRemoveUntil(context, MaterialPageRoute(builder: (context) => page), (route) => false);
    }
  }
}