import requests
from typing import Union,List
import asyncio
from fastapi import FastAPI


app = FastAPI()

@app.post('/')
def get_asset_id():
    return {"message":"hello world"}



# header = {"x-functions-key":"yeK7CM/Pj2vA3MFpuBxIFX7QIl1cKFOiviZaOjtVCrTq0VUzKeQjfw=="}
# res = requests.get("http://tech-assessment.vnext.com.au/api/devices/assetId/DVID111111",headers=header)
# print(res.text)
