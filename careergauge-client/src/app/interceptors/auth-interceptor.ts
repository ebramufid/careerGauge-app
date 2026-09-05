import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  if (req.url.startsWith('http://localhost:5087/api/')) {
    req = req.clone({
      withCredentials: true
    });
  }

  return next(req);
};