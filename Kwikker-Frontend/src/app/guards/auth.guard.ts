import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router); // Inject Router

  if (localStorage.getItem('accessToken')) {
    return true; // Access is allowed
  }

  // Redirect to login if no token
  router.navigate(['login']);
  return false;
};
