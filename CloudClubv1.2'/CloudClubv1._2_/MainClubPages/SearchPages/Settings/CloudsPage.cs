﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using PCLStorage;

using Xamarin.Forms;
using CloudClubv1._2_;
using Backend;
namespace FrontEnd
{
    public class CloudsPage : ContentPage
    {

        ColorHandler ch;
        List<Cloud> cloudList;
        List<Cloud> currentCloudField;
        public CloudsPage(List<Cloud> cloudList, List<string> savedClouds)
        {
            ch = new ColorHandler();
            currentCloudField = new List<Cloud>();
            cloudList = new List<Cloud>();
            var tableSection = new TableSection();
            // BackgroundColor = ch.fromStringToColor("black");
            Title = "Clouds";

            for (int i = 0; i < savedClouds.Count; i++)
            {
                bool saved = false;
                for (int j = 0; j < cloudList.Count; j++)
                {
                    if (savedClouds[i].Equals(cloudList[j].Title))
                    {
                        saved = true;
                        currentCloudField.Add(cloudList[j]);
                    }

                }


                var s = new CustomSwitch
                {
                   
                    Text = savedClouds[i],
                    
                    On = saved,


                };
                s.OnChanged += S_OnChanged;
                tableSection.Add(s);
            }

            var cloudSwitchTable = new TableView
            {
                Root = new TableRoot(),
                BackgroundColor = ch.fromStringToColor("white")

            };
            cloudSwitchTable.Root.Add(tableSection);
            Content = cloudSwitchTable;
        }

        private async void S_OnChanged(object sender, ToggledEventArgs e)
        {
            return;
            var s = (SwitchCell)sender;
            var saveFileKey = new SaveFileDictionary();

            var fileSystem = FileSystem.Current.LocalStorage;
            var exists = fileSystem.CheckExistsAsync("PhoneData.txt");

            if (s.On == true)
            {
                for (int i = 0; i < cloudList.Count; i++)
                {
                    if (s.On == true)
                    {
                        if (cloudList[i].Title == s.Text)
                        {
                            await App.dbWrapper.JoinCloud(cloudList[i].Id);
                            currentCloudField.Add(cloudList[i]);
                        }
                    }
                    else
                    {

                    }
                }

            }
            else
            {
                for (int i = 0; i < cloudList.Count; i++)
                {
                    if (cloudList[i].Title == s.Text)
                    {
                        if (currentCloudField.Contains(cloudList[i]))
                        {
                            currentCloudField.Remove(cloudList[i]);
                        }
                        else
                        {
                            throw new Exception("Cloud Not Contained In field");
                        }
                        //await App.dbWrapper.(cloudList[i].Id);
                    }
                }
                for (int i = 0; i < currentCloudField.Count; i++)
                {
                    await App.dbWrapper.SetCurrentCloud(currentCloudField[i].Id);
                }
            }
            if (exists.Equals(ExistenceCheckResult.FileExists))
            {
                var file = await fileSystem.GetFileAsync("PhoneData.txt");
                var text = await file.ReadAllTextAsync();
                var fileLines = text.Split('\n');
                var savedClouds = fileLines[saveFileKey.dict["CLOUDREGION"]].Split(',').ToList<string>();
                if (s.On == true) //Add cloud to file
                {
                    bool notYetSaved = false;
                    for (int i = 0; i < savedClouds.Count; i++)
                    {
                        if (!savedClouds[i].Equals(s.Text))
                        {
                            notYetSaved = true;

                        }

                    }
                    if (notYetSaved)
                    {
                        savedClouds.Add(s.Text);
                        string saveString = "";
                        for (int j = 0; j < savedClouds.Count; j++)
                        {
                            saveString += savedClouds + ",";
                        }

                        fileLines[saveFileKey.dict["CLOUDREGION"]] = saveString;
                        saveString = "";
                        for (int j = 0; j < fileLines.Length; j++)
                        {
                            saveString += fileLines[j] + "\n";
                        }
                        await file.WriteAllTextAsync(saveString);


                    }
                }
                else //Remove cloud from file
                {
                    if (savedClouds.Count == 0) throw new IndexOutOfRangeException("No Clouds Were In List");

                    bool aleadySaved = true;
                    for (int i = 0; i < savedClouds.Count; i++)
                    {
                        if (!savedClouds[i].Equals(s.Text))
                        {
                            aleadySaved = false;

                        }
                    }
                    if (aleadySaved) //TODO make compile savestring method
                    {
                        var index = savedClouds.IndexOf(s.Text);
                        savedClouds.RemoveAt(index);
                        var saveString = "";
                        for (int i = 0; i < savedClouds.Count; i++)
                        {
                            saveString += savedClouds[i] + ",";
                        }

                        fileLines[saveFileKey.dict["CLOUDREGION"]] = saveString;
                        saveString = "";
                        for (int i = 0; i < fileLines.Length; i++)
                        {
                            saveString += fileLines[i] + "\n";
                        }
                        await file.WriteAllTextAsync(saveString);
                    }

                }
            }
            else
            {
                throw new System.IO.FileNotFoundException("PhoneData.txt");
            }

        }
    }
}
