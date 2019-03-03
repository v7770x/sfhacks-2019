using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class GetData : MonoBehaviour
{
    public int heartRate;
    // Start is called before the first frame update
    void Start()
    {
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sf-hacks-19.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        DoThis();
       
    }

    private void DoThis()
    {
        FirebaseDatabase.DefaultInstance
       .GetReference("users")
       .ValueChanged += HandleValueChanged;

        //Debug.Log(FirebaseDatabase.DefaultInstance.GetReference("users"));
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Debug.Log(args.Snapshot.Child("Bob").Child("curr_hr").Child("val").Child("heartRate").Value);
        heartRate = int.Parse(args.Snapshot.Child("Bob").Child("curr_hr").Child("val").Child("heartRate").Value.ToString());
        Debug.Log(heartRate);
    }
}
