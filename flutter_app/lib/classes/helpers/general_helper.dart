

import 'package:fluttertoast/fluttertoast.dart';

abstract class GeneralUtil {
  /// Show a toast to the user
  static void showToast(String message) {
    Fluttertoast.showToast(msg: message, toastLength: Toast.LENGTH_LONG);
  }
}