import { NoZeroPipe } from './no-zero.pipe';

describe('NoZeroPipe', () => {
  it('create an instance', () => {
    const pipe = new NoZeroPipe();
    expect(pipe).toBeTruthy();
  });
});
