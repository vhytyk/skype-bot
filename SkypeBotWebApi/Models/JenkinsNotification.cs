using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SkypeBotWebApi.Models
{
    public class JenkinsBuildNotification
    {
        [JsonProperty("full_url")]
        public string FullUrl { get; set; }
        public int Number { get; set; }
        public string Phase { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
    }
    public class JenkinsNotification
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public JenkinsBuildNotification Build { get; set; }

//        {
//    "name": "asgard",
//    "url": "job/asgard/",
//    "build": {
//        "full_url": "http://localhost:8080/job/asgard/18/",
//        "number": 18,
//        "phase": "COMPLETED",
//        "status": "SUCCESS",
//        "url": "job/asgard/18/",
//        "scm": {
//            "url": "https://github.com/evgeny-goldin/asgard.git",
//            "branch": "origin/master",
//            "commit": "c6d86dc654b12425e706bcf951adfe5a8627a517"
//        },
//        "artifacts": {
//            "asgard.war": {
//                "archive": "http://localhost:8080/job/asgard/18/artifact/asgard.war"
//            },
//            "asgard-standalone.jar": {
//                "archive": "http://localhost:8080/job/asgard/18/artifact/asgard-standalone.jar",
//                "s3": "https://s3-eu-west-1.amazonaws.com/evgenyg-bakery/asgard/asgard-standalone.jar"
//            }
//        }
//    }
//}
    }
}