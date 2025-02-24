const UserSessionValidation = async (): Promise<boolean> => {
  try {
    const response = await fetch('/api/Accounts/authenticated', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include'
    });
    if (!response.ok) {
      if (response.status === 401) {
        const errorData = await response.json();
        const errorMesssage = errorData.message || 'Unauthorized';
        throw new Error(errorMesssage);
      }
      if (response.status === 400) {
        return false;
      }
      return false;
    }
    return true;

  }
  catch (e: any) {
    console.error("Login Check Error", e);
    alert(`Error checking login status ${e.message}`);
    return false;
  }
}


export default UserSessionValidation;
