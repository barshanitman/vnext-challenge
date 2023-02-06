import requests
from typing import Union,List
import asyncio
from fastapi import FastAPI
from model_types import RequestBodyType

app = FastAPI()

@app.post('/assetId')
def get_asset_id(request:RequestBodyType):
    
    return {"message":request}



# header = {"x-functions-key":"yeK7CM/Pj2vA3MFpuBxIFX7QIl1cKFOiviZaOjtVCrTq0VUzKeQjfw=="}
# res = requests.get("http://tech-assessment.vnext.com.au/api/devices/assetId/DVID111111",headers=header)
# print(res.text)
