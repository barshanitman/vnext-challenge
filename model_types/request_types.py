from typing import Union,List,Dict
from pydantic import BaseModel

class RequestBodyType(BaseModel):
    correlationid:str
    devices:List[Dict[str,str]]

    






