package com.djdm.unity;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.util.Log;

public class MyPlugin
{
    static final MyPlugin instance = new MyPlugin();

    static final String LOGTAG = "DJDM_Plugin";

    long startTime;

    public static Activity mainActivity;

    public static MyPlugin getInstance() { return instance; }

    MyPlugin()
    {
        Log.i(LOGTAG, "Created MyPlugin");
        startTime = System.currentTimeMillis();
    }

    public interface AlertViewCallback
    {
        public void OnButtonTapped(int id);
    }

    public void writeHighscore(float score, Context context)
    {
        File path = mainActivity.getFilesDir();
        File file = new File(path, "highscore.txt");

        try
        {
            FileOutputStream stream = new FileOutputStream(file);
            try
            {
                stream.write(Float.toString(score).getBytes());
            }
            finally
            {
                stream.close();
            }
        }
        catch (IOException e)
        {
            Log.e("Exception", "File write failed: " + e.toString());
        }
    }

    public float readHighscore(Context context)
    {
        File path = context.getFilesDir();

        File file = new File(path, "highscore.txt");
        if (!file.exists()) return 0f;

        int length = (int) file.length();
        byte[] bytes = new byte[length];

        try
        {
            FileInputStream stream = new FileInputStream(file);
            try
            {
                stream.read(bytes);
            }
            finally
            {
                stream.close();
            }
        }
        catch (IOException e)
        {
            Log.e("Exception", "File write failed: " + e.toString());
        }

        String score = new String(bytes);
        return Float.parseFloat(score);
    }

    public void showAlertView(final String[] strings, final AlertViewCallback callback)
    {
        if (strings.length < 3)
        {
            Log.i(LOGTAG, "At least 3 strings needed, got " + strings.length);
            return;
        }

        DialogInterface.OnClickListener myClickListener = new DialogInterface.OnClickListener()
        {
            @Override
            public void onClick(DialogInterface dialogInterface, int id)
            {
                dialogInterface.dismiss();
                Log.i(LOGTAG, "Tapped: " + id);
                callback.OnButtonTapped(id);
            }
        };

        AlertDialog alertDialog = new AlertDialog.Builder(mainActivity)
                .setTitle(strings[0])
                .setMessage(strings[1])
                .setCancelable(false)
                .create();
        alertDialog.setButton(AlertDialog.BUTTON_NEUTRAL, strings[2], myClickListener);
        if (strings.length > 3)
            alertDialog.setButton(AlertDialog.BUTTON_NEGATIVE, strings[3], myClickListener);
        if (strings.length > 4)
            alertDialog.setButton(AlertDialog.BUTTON_POSITIVE, strings[4], myClickListener);

        alertDialog.show();
    }
}