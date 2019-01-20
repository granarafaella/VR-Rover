package com.example.malon_000.hacked2019;

import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.hardware.usb.UsbAccessory;
import android.hardware.usb.UsbDevice;
import android.hardware.usb.UsbDeviceConnection;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.hardware.usb.UsbManager;

import com.felhr.usbserial.SerialInputStream;
import com.felhr.usbserial.SerialOutputStream;
import com.felhr.usbserial.UsbSerialDevice;
import com.felhr.usbserial.UsbSerialInterface;

import java.io.UnsupportedEncodingException;
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
                    if (connected) {


                        status_list.add("already connected");
                        ArrayAdapter<String> status_adapter = new ArrayAdapter<String>(getBaseContext(), android.R.layout.simple_list_item_1, status_list);
                        status_list_view.setAdapter(status_adapter);
                        UsbDeviceConnection connection = usbManager.openDevice(device);
                        UsbSerialDevice serialPort = UsbSerialDevice.createUsbSerialDevice(device, connection);



                        if (serialPort != null) {

                            serialPort.syncOpen();
                            serialPort.setBaudRate(115200);
                            status_list.add("Serial port open");
                            status_adapter = new ArrayAdapter<String>(getBaseContext(), android.R.layout.simple_list_item_1, status_list);
                            status_list_view.setAdapter(status_adapter);
                            serialPort.setDataBits(UsbSerialInterface.DATA_BITS_8);
                            serialPort.setParity(UsbSerialInterface.PARITY_ODD);
                            serialPort.setFlowControl(UsbSerialInterface.FLOW_CONTROL_OFF);
                            SerialOutputStream command_stream = serialPort.getOutputStream();
                            SerialInputStream error = serialPort.getInputStream();


                            command_stream.write("f".getBytes());
                            command_stream.write("\n".getBytes());
                            status_list.add("moved forward");
                            status_adapter = new ArrayAdapter<String>(getBaseContext(), android.R.layout.simple_list_item_1, status_list);
                            status_list_view.setAdapter(status_adapter);


                        }
                    }else{


                        Set<Map.Entry> devices = usbDevices.entrySet();

                        for (Map.Entry entry : devices) {
                            device = (UsbDevice) entry.getValue();
                            status_list.add(Integer.toString(device.getVendorId()));
                            ArrayAdapter<String> status_adapter = new ArrayAdapter<String>(getBaseContext(), android.R.layout.simple_list_item_1, status_list);
                            status_list_view.setAdapter(status_adapter);
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
        clear.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                status_list.clear();
                ArrayAdapter<String> status_adapter = new ArrayAdapter<String>(getBaseContext(), android.R.layout.simple_list_item_1, status_list);
                status_list_view.setAdapter(status_adapter);
            }
        });
    }
}