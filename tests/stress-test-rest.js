import http from "k6/http";
import { check, sleep } from "k6";

export let options = {
  stages: [
    { duration: "1m", target: 10 }, // ramp up to 10 users
    { duration: "3m", target: 10 }, // stay at 10 users for 3 minutes
    { duration: "1m", target: 0 }, // ramp down to 0 users
  ],
};

function getRandomCnpj() {
  return Math.floor(Math.random() * 100000000000000)
    .toString()
    .padStart(14, "0");
}

export default function () {
  let cnpj = getRandomCnpj();
  let res = http.get(`http://localhost:5001/companies/${cnpj}/rest`);
  check(res, { "status was 200": (r) => r.status == 200 });
  sleep(1);
}
