import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 5,
    duration: '30s',
    thresholds: {
        http_req_failed: ['rate<0.01'],
        http_req_duration: ['p(95)<800'],
    },
};

export default function () {
    const response = http.get('http://localhost:5174/');

    check(response, {
        'Сторінка доступна (200)': (r) => r.status === 200,
    });

    sleep(1);
}