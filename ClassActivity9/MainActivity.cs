using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using SQLiteDB.Resources.Model;
using System.Collections.Generic;
using SQLiteDB.Resources.Helper;
using System;

namespace ClassActivity9
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ListView lstViewData;
        List<Shopping> listSource = new List<Shopping>();
        Database db;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.activity_main);
            //Create Database  
            db = new Database();
            db.createDatabase();
            var edtName = FindViewById<EditText>(Resource.Id.txtView_Name);
            lstViewData = FindViewById<ListView>(Resource.Id.listView);
            var btnAdd = FindViewById<Button>(Resource.Id.btnShowDialog);
            //Load Data  
            LoadData();
            Button button = FindViewById<Button>(Resource.Id.btnShowDialog);
            float startX = 0;
            float webViewWidth = 0;

            lstViewData.Touch += (sender, e) =>
            {
                if (e.Event.Action == Android.Views.MotionEventActions.Down)
                {
                    webViewWidth = lstViewData.Width;
                    startX = e.Event.GetX();
                }
                if (e.Event.Action == Android.Views.MotionEventActions.Up)
                {
                    float movement = e.Event.GetX() - startX;
                    float offset = webViewWidth / 2;

                    if (Math.Abs(movement) > offset)
                    {
                        if (movement < 0)
                        {
                            //Shopping shop = new Shopping()
                            //{
                            //    Id = int.Parse(edtName.Tag.ToString()),
                            //    Name = edtName.Text,
                            //};
                            //db.removeTable(shop);
                            Toast.MakeText(this, "Left ", ToastLength.Short).Show();
                        }
                    }
                }
                e.Handled = false;
            };
            button.Click += delegate
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View view = layoutInflater.Inflate(Resource.Layout.user_input_dialog_box, null);
                Android.Support.V7.App.AlertDialog.Builder alertbuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertbuilder.SetView(view);
                var userdata = view.FindViewById<EditText>(Resource.Id.editText);
                alertbuilder.SetCancelable(false)
                .SetPositiveButton("Submit", delegate
                {
                    Shopping shopping = new Shopping()
                    {
                        Name = userdata.Text
                    };
                    db.insertIntoTable(shopping);
                    LoadData();
                    Toast.MakeText(this, "Submit Input: " + userdata.Text, ToastLength.Short);
                })
                .SetNegativeButton("Cancel", delegate
                {
                    alertbuilder.Dispose();
                });
                Android.Support.V7.App.AlertDialog dialog = alertbuilder.Create();
                dialog.Show();
            };
            }
        private void LoadData()
            {
                listSource = db.selectTable();
                var adapter = new ListViewAdapter(this, listSource);
                lstViewData.Adapter = adapter;
            }
        };
    }