from flask import Flask
import fitbit
import gather_keys_oauth2 as Oauth2
import pandas as pd 
import datetime
from flask import request
from flask import render_template
import datetime
from time import sleep
import pyrebase

app = Flask(__name__)


config = {
	"apiKey": "AIzaSyDyfbWP_I3kHyH8h81WypU_sIY0pFhUtqE",
    "authDomain": "sf-hacks-19.firebaseapp.com",
    "databaseURL": "https://sf-hacks-19.firebaseio.com",
	"storageBucket": "sf-hacks-19.appspot.com",
	"serviceAccount": "./auth/sf-hacks-19-firebase-adminsdk-3592h-8129e93e65.json"
}
firebase = pyrebase.initialize_app(config)

db=firebase.database()

@app.route('/')
def hello_world():
    users_array = get_data()
    return render_template('index.html', data=users_array)

# CLIENT_ID = '22DKKD'
# CLIENT_SECRET = '2c40926ab4cccef6d86468db859c5967'

def get_data():
    users = db.child('users').get()
    users_array = []
    for user in users.each():
        user_datum = user.val()
        # print(user_datum)
        # users_array.append(user.val())
        data_points = []
        if(user_datum==1):
            continue
        # print(user)
        # print(user_datum.get('hr_time_data'))
        low_percent = 0
        mid_percent = 0
        high_percent = 0
        total = 0

        local_avg = 0
        local_count = 0
        num_pts = 0
        for v in user_datum['hr_time_data'].values():
            # print(v)
            # if(v!='test'):
            try:
                hr = v["hr"]["heartRate"]
                
                if hr >90:
                    high_percent+=1
                elif hr >75:
                    mid_percent +=1
                else:
                    low_percent +=1
                total+= hr
                num_pts+=1

                local_count+=1
                local_avg+=hr

                if(local_count>59 or num_pts==0):
                    tim = v["time"].split(":")
                    tim = int(tim[0])*60+int(tim[1])
                    print(tim)
                    data_points.append({"x":tim,"y":local_avg/local_count})
                    local_avg=0
                    local_count=0
            except:
                print("sucks")
            
        avg = total/ num_pts
                # print({"x":v["time"],"y":v["hr_data"]["heartRate"]})
        high_percent = high_percent*1.0/num_pts*100
        mid_percent = mid_percent*1.0/num_pts*100
        low_percent = low_percent*1.0/num_pts*100
        users_array.append({"h":high_percent, "m":mid_percent, "l":low_percent,"data":data_points, "avg": avg})
    return users_array
    
    # print(users_array)

# get_data()

# server = Oauth2.OAuth2Server(CLIENT_ID, CLIENT_SECRET)
# server.browser_authorize()
# ACCESS_TOKEN = str(server.fitbit.client.session.token['access_token'])
# REFRESH_TOKEN = str(server.fitbit.client.session.token['refresh_token'])
# auth2_client = fitbit.Fitbit(CLIENT_ID, CLIENT_SECRET, oauth2=True, access_token=ACCESS_TOKEN, refresh_token=REFRESH_TOKEN)


# yesterday = str((datetime.datetime.now() - datetime.timedelta(days=1)).strftime("%Y%m%d"))
# yesterday2 = str((datetime.datetime.now() - datetime.timedelta(days=1)).strftime("%Y-%m-%d"))
# today = str(datetime.datetime.now().strftime("%Y%m%d"))

# print(yesterday)

# while True:
#     fit_statsHR = auth2_client.intraday_time_series('activities/heart', base_date='today', detail_level='1sec')

#     print(fit_statsHR)
#     sleep(1000)