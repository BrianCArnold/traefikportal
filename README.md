# traefikportal

Pulls the list of all HTTP routes from Traefik and provides a set of Bootstrap Cards with each one.

Firstly, it must be provided with one Environment variable, the root of your Traefik Status site, and it must be able to access it.

In my case, Traefik's Compose file looks pretty similar to this, and is deployed on a cluster with the manager node at 192.168.0.10.

Note that I am useing Traefik in *Swarm Mode*, so the labels are on the 'deploy' section, not directly under the service name.

(not my actual data, obviously, that's been replaced. But everything I changed will just be called "REPLACE_ME" if you want to use this as a template)


```
version: "3.8"

services:
  traefik:
    image: "traefik:latest"
    ports:
      - "80:80"
      - "443:443"
      - "1080:8080"
    environment:
      NAMECOM_USERNAME: "REPLACE_ME"
      NAMECOM_API_TOKEN: "REPLACE_MEREPLACE_MEREPLACE_MEREPLACE_ME"
      NAMECOM_SERVER: "api.name.com"
    volumes:
      - "/var/docker/config/REPLACE_ME/REPLACE_ME/REPLACE_ME:/etc/traefik"
      - "/var/run/docker.sock:/var/run/docker.sock:ro"
    networks:
      - "webhosts"
    
  traefik-portal:
    image: "briancarnold/traefik-portal:latest"
    deploy:
      labels: 
        - "traefik.enable=true"
        - 'traefik.http.routers.Services-Portal.rule=Host(`portal.REPLACE_ME.com`)'
        - 'traefik.http.routers.Services-Portal.tls=true'
        - "traefik.http.routers.Services-Portal.entrypoints=secure"
        - "traefik.http.routers.Services-Portal.service=Services-Portal-svc"
        - "traefik.http.services.Services-Portal-svc.loadBalancer.server.port=80"
    environment:
      TRAEFIK_ROOT_URL: "http://192.168.0.10:1080"
    networks:
      - "webhosts"

networks:
  webhosts:
    external: true
```


To generate the title of each card, it takes the name of the router (without the provider name) and splits it by dashes and underscores, and uppercases the first letter of each word.


|  Route Name                     |  Group  | Name                             |
|---------------------------------|---------|----------------------------------|
|  Consume-Some-Shows@docker      | Consume | Some Shows                       |
|  Consume_Some_Radio@docker      | Consume | Some Radio                       |
|  Manage-Your_Shows@docker       | Manage  | Your Shows                       |
|  Manage_Your-Radio@docker       | Manage  | Your Radio                       |

Which provides something akin to the following:


|  Consume |
|----------|
| <table><thead><tr><th>Shows</th><th>Radio</th></tr></thead><tbody><tr><td>[https://...](https://github.com/BrianCArnold/traefikportal)</td><td>[https://...](https://github.com/BrianCArnold/traefikportal)</td></tr></tbody></table> |

| Manage |
|-----|
| <table><thead><tr><th>Your Shows</th><th>Your Radio</th></tr></thead><tbody><tr><td>[https://...](https://github.com/BrianCArnold/traefikportal)</td><td>[https://...](https://github.com/BrianCArnold/traefikportal)</td></tr></tbody></table> |


Additionally, for the moment, it assumes all routes go to https:// on port 443.

| Traefik Rule                      | Link URL                         |
|-----------------------------------|----------------------------------|
| Host(\`shows.example-site.com\`)  | https://shows.example-site.com/  |

