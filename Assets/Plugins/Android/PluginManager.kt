package com.bss.download

import android.app.*
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.database.Cursor
import android.net.Uri
import android.os.Build
import android.widget.Toast
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat
import com.unity3d.player.UnityPlayer
import com.unity3d.player.UnityPlayerActivity
import java.io.File

class PluginManager {
    val PACKAGE_NAME="com.bss.download"
    val NOTIFICATION_IMAGE_NAME="icon_lc"
    val CHANNEL_ID : String = "bss_channel_default"
    val NOTIFICATION_ID : Int = 153153

    var gameObjectName=""
    var callbackFuntionName=""
    var lastDownId : Long=-1
    var isInit :Boolean=false


    fun Initialize(_gameObjectName :String,_callbackFuntionName : String) {
        if (isInit) {
            return
        }
        isInit=true
        gameObjectName=_gameObjectName
        callbackFuntionName=_callbackFuntionName

        //Channel Init
        if (Build.VERSION.SDK_INT>=Build.VERSION_CODES.O) {
            val channelId=CHANNEL_ID
            val channel=NotificationChannel(channelId,"Default Channel",NotificationManager.IMPORTANCE_DEFAULT)
            val notificationManager = UnityPlayer.currentActivity.getSystemService(NotificationManager::class.java)
            notificationManager.createNotificationChannel(channel)
        }

        //BroadCast Receiver Regist
        val intentFilter=IntentFilter(DownloadManager.ACTION_DOWNLOAD_COMPLETE)
        val broadcastReceiver =object :BroadcastReceiver() {
            override fun onReceive(context: Context?, intent: Intent?) {
                when (intent?.action) {
                    DownloadManager.ACTION_DOWNLOAD_COMPLETE->{
                        val downloadManager=UnityPlayer.currentActivity.getSystemService(Context.DOWNLOAD_SERVICE)
                        SendNotification("다운로드 완료","실행하세요.")
                    }
                }
            }
        }
        UnityPlayer.currentActivity.registerReceiver(broadcastReceiver,intentFilter)
    }

    fun ShowToast(text : String) {
        Toast.makeText(UnityPlayer.currentActivity,text,Toast.LENGTH_SHORT).show()
    }

    fun SendNotification(title : String , content:String) {
        val intent=Intent(UnityPlayer.currentActivity,UnityPlayer.currentActivity.javaClass)
        intent.flags= Intent.FLAG_ACTIVITY_CLEAR_TOP or Intent.FLAG_ACTIVITY_SINGLE_TOP
        val pendingIntent = PendingIntent.getActivity(UnityPlayer.currentActivity.baseContext, 0, intent, 0)

        val res=UnityPlayer.currentActivity.resources
        val icon=res.getIdentifier(NOTIFICATION_IMAGE_NAME,"drawable",PACKAGE_NAME)


        val builder= NotificationCompat.Builder(UnityPlayer.currentActivity,CHANNEL_ID)
            .setSmallIcon(icon)
            .setContentTitle(title)
            .setContentText(content)
            .setAutoCancel(true)
            .setContentIntent(pendingIntent)

        builder.priority=NotificationCompat.PRIORITY_DEFAULT


        val notificationManager =NotificationManagerCompat.from(UnityPlayer.currentActivity)
        notificationManager.notify(NOTIFICATION_ID,builder.build())
    }


    fun DownloadData(url : String, fileName : String) : Long {
        val downloadManager=UnityPlayer.currentActivity.getSystemService(Context.DOWNLOAD_SERVICE)
        val file=File(UnityPlayer.currentActivity.getExternalFilesDir(null),fileName)

        val req=DownloadManager.Request(Uri.parse(url))
            .setTitle("$fileName Downloading... ")
            .setDescription("Please wait...")
            .setNotificationVisibility(DownloadManager.Request.VISIBILITY_VISIBLE)
            .setDestinationUri(Uri.fromFile(file))
            .setAllowedOverMetered(true)
            .setAllowedOverRoaming(true)

        if (downloadManager is DownloadManager) {
            lastDownId = downloadManager.enqueue(req)
        }

        return lastDownId
    }

    fun GetDownloadStatus(downloadId : Long) : Int {
        val downloadManager=UnityPlayer.currentActivity.getSystemService(Context.DOWNLOAD_SERVICE)
        val query = DownloadManager.Query()
        query.setFilterById(downloadId)
        if (downloadManager is DownloadManager) {
            val cursor = downloadManager.query(query)
            if (!cursor.moveToFirst()) {//Query is empty
                return -1
            }
            return cursor.getInt(cursor.getColumnIndex(DownloadManager.COLUMN_STATUS))
        }
        return -1
    }


}