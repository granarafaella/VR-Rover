package com.example.malon_000.hacked2019;

import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.hardware.usb.UsbAccessory;
import android.hardware.usb.UsbDevice;
import android.hardware.usb.UsbDeviceConnection;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.hardware.usb.UsbManager;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;



public class MainActivity extends AppCompatActivity {
    private ListView status_list_view;
    private ArrayList<String> status_list;
    private UsbManager usbManager;
    private Button reconnect;
    private Button clear;
    private static final String ACTION_USB_PERMISSION = "com.android.example.USB_PERMISSION";
    private UsbDevice device;
    private Boolean connected = false;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        status_list= new ArrayList<String>();
        setContentView(R.layout.activity_main);
        Context context = getBaseContext();
        usbManager= context.getSystemService(UsbManager.class);
        status_list_view = (ListView) findViewById(R.id.ErrorList);
        reconnect = (Button) findViewById(R.id.reconnect);

        reconnect.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                    HashMap usbDevices = usbManager.getDeviceList();
                    if (!usbDevices.isEmpty()) {
                        if (connected){
                            status_list.add("already connected");
                            ArrayAdapter<String> status_adapter = new ArrayAdapter<String>(getBaseContext(), android.R.layout.simple_list_item_1, status_list);
                            status_list_view.setAdapter(status_adapter);
                        }
                        else {
                            Set<Map.Entry> devices = usbDevices.entrySet();

                            for (Map.Entry entry : devices) {
                                status_list.add("detected device :" + entry.getValue().toString());
                                ArrayAdapter<String> status_adapter = new ArrayAdapter<String>(getBaseContext(), android.R.layout.simple_list_item_1, status_list);
                                status_list_view.setAdapter(status_adapter);
                                device = (UsbDevice) entry.getValue();
                                PendingIntent pi = PendingIntent.getBroadcast(getBaseContext(), 0, new Intent(ACTION_USB_PERMISSION), 0);
                                usbManager.requestPermission(device, pi);
                                status_list.add("requested permission");
                                ArrayAdapter<String> status_adapter2 = new ArrayAdapter<String>(getBaseContext(), android.R.layout.simple_list_item_1, status_list);
                                status_list_view.setAdapter(status_adapter2);
                                connected = true;
                            }
                        }
                    } else {
                        status_list.add("No device detected");
                        ArrayAdapter<String> status_adapter = new ArrayAdapter<String>(getBaseContext(), android.R.layout.simple_list_item_1, status_list);
                        status_list_view.setAdapter(status_adapter);
                    }

            }
        });
        clear = (Button) findViewById(R.id.clear_button);
        clear.setOnClickListener(new View.OnClickListener(){
            @Override
            public void onClick(View view){
                status_list.clear();
                ArrayAdapter<String> status_adapter = new ArrayAdapter<String>(getBaseContext(),android.R.layout.simple_list_item_1,status_list);
                status_list_view.setAdapter(status_adapter);
            }
        });
        HashMap usbDevices = usbManager.getDeviceList();
        if (!usbDevices.isEmpty()){

            Set<Map.Entry> devices = usbDevices.entrySet();

            for (Map.Entry entry : devices){
                status_list.add("detected device :" + entry.getValue().toString());
                ArrayAdapter<String> status_adapter = new ArrayAdapter<String>(this,android.R.layout.simple_list_item_1,status_list);
                status_list_view.setAdapter(status_adapter);
                device = (UsbDevice) entry.getValue();
                PendingIntent pi = PendingIntent.getBroadcast(this, 0, new Intent(ACTION_USB_PERMISSION),0);
                usbManager.requestPermission(device,pi);
                status_list.add("requested permission");
                ArrayAdapter<String> status_adapter2 = new ArrayAdapter<String>(this,android.R.layout.simple_list_item_1,status_list);
                status_list_view.setAdapter(status_adapter2);
            }



        }
        else{
            status_list.add("No device detected");
            ArrayAdapter<String> status_adapter = new ArrayAdapter<String>(this,android.R.layout.simple_list_item_1,status_list);
            status_list_view.setAdapter(status_adapter);
        }


    }
}

