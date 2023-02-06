import requests

header = {"x-functions-key":"yeK7CM/Pj2vA3MFpuBxIFX7QIl1cKFOiviZaOjtVCrTq0VUzKeQjfw=="}
res = requests.get("http://tech-assessment.vnext.com.au/api/devices/assetId/DVID00000123",headers=header)
print(res.text)
